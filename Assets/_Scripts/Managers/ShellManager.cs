using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Core.Grid;
using _Scripts.Core.Tile;
using _Scripts.Data.EndingUI;
using _Scripts.Utils.Event_Bus;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Managers
{
    public class ShellManager : Singleton<ShellManager>
    {
        [Header("Shell")] [SerializeField] private GameObject shell;
        [SerializeField] private GameObject additionSlot;
        [SerializeField] private float cellSize = 0.5f;

        public List<Transform> slots = new List<Transform>();
        public List<TileCell> tilesInShell = new List<TileCell>();

        [Header("Return Slots")] [SerializeField]
        private GameObject returnShell;
        public List<Transform> returnSlots = new List<Transform>();

        [Header("Board (for win check)")] [SerializeField]
        private GridSpawner gridSpawner;

        private readonly List<TileCell> _destroyingCells = new List<TileCell>();
        private bool _isDestroying = false;
        private bool _endingFired = false;
        private int _fullSlots = 0;
        private int _cellGroupNums;

        public event Action<EndingType> openEndingUI;

        private void OnEnable() => EventBus.Subscribe<TileClickEvent>(AddTile);
        private void OnDisable() => EventBus.Unsubscribe<TileClickEvent>(AddTile);

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        private void Init()
        {
            if (additionSlot.activeInHierarchy)
                additionSlot.SetActive(false);

            slots = shell.GetComponentsInChildren<Transform>()
                .Where(t => t != shell.transform).ToList();
            slots[slots.Count - 1].GetComponent<SpriteRenderer>().enabled = false;

            returnSlots = returnShell.GetComponentsInChildren<Transform>()
                .Where(t => t != returnShell.transform).ToList();
        }

        public void InitGroupCount(int cellGroupNums)
        {
            _cellGroupNums = cellGroupNums;
        }

        #region Add tile

        private void AddTile(TileClickEvent evt)
        {
            TileCell tile = evt.TileCell;

            _fullSlots = _isDestroying ? slots.Count : slots.Count - 1;
            if (tilesInShell.Count >= _fullSlots)
                return;

            int index = FindInsertIndex(tile.ID);
            tilesInShell.Insert(index, tile);

            Sequence sequence = RelayoutShell();
            sequence.OnComplete(() =>
            {
                CheckMatching(tile.ID);
                if (!_isDestroying)
                    CheckLose();
            });
        }

        // Chèn tile mới ngay sau nhóm cùng ID (giữ các tile cùng loại liền nhau),
        // hoặc về cuối nếu chưa có tile cùng ID nào.
        private int FindInsertIndex(int id)
        {
            int last = -1;
            for (int i = 0; i < tilesInShell.Count; i++)
                if (tilesInShell[i].ID == id)
                    last = i;

            return last == -1 ? tilesInShell.Count : last + 1;
        }

        // Dời toàn bộ tile về đúng slot theo index hiện tại của chúng.
        private Sequence RelayoutShell()
        {
            Sequence seq = DOTween.Sequence();
            for (int i = 0; i < tilesInShell.Count; i++)
            {
                tilesInShell[i].transform.DOKill();
                seq.Join(tilesInShell[i].transform
                    .DOMove(slots[i].position, 0.35f).SetEase(Ease.OutExpo));
            }
            return seq;
        }

        #endregion

        #region Matching

        private void CheckMatching(int id)
        {
            int count = 0;
            foreach (var t in tilesInShell)
                if (t.ID == id) count++;

            if (count < 3) return;

            _isDestroying = true;
            RemoveMatching(id);
        }

        private void RemoveMatching(int id)
        {
            // Lấy ĐÚNG 3 tile cùng ID theo thứ tự thật, bỏ qua tile đang bị huỷ dở.
            var toDestroy = new List<TileCell>();
            foreach (var t in tilesInShell)
            {
                if (t.ID == id && !_destroyingCells.Contains(t))
                {
                    toDestroy.Add(t);
                    if (toDestroy.Count == 3) break;
                }
            }

            if (toDestroy.Count < 3)
            {
                _isDestroying = _destroyingCells.Count > 0;
                return;
            }

            _destroyingCells.AddRange(toDestroy);

            Sequence sq = DOTween.Sequence();
            float delay = 0f;
            foreach (var cell in toDestroy)
            {
                cell.transform.DOKill();
                sq.Join(cell.transform.DOPunchScale(
                    new Vector3(0.2f, 0.2f, 0f), 0.3f, 3, 0.8f).SetDelay(delay));
                delay += 0.05f;
            }

            sq.AppendCallback(() =>
            {
                foreach (var cell in toDestroy)
                {
                    tilesInShell.Remove(cell);
                    _destroyingCells.Remove(cell);
                    Destroy(cell.gameObject);
                }
            });

            sq.OnComplete(() =>
            {
                _isDestroying = _destroyingCells.Count > 0;
                RelayoutShell();
                if (_cellGroupNums > 0) _cellGroupNums--;
                CheckComplete();
            });
        }

        #endregion

        #region Win / Lose

        private void CheckLose()
        {
            if (_endingFired) return;
            if (tilesInShell.Count == _fullSlots)
            {
                _endingFired = true;
                openEndingUI?.Invoke(EndingType.Lose);
            }
        }

        private void CheckComplete()
        {
            if (_endingFired) return;
            if (_cellGroupNums > 0) return;

            bool boardEmpty = gridSpawner == null || gridSpawner.ActiveCells.Count == 0;
            bool shellEmpty = tilesInShell.Count == 0;

            if (boardEmpty && shellEmpty)
            {
                _endingFired = true;
                openEndingUI?.Invoke(EndingType.Win);
            }
        }

        #endregion

        #region Slot / Return shell

        public void AddSlot()
        {
            Sequence sequence = DOTween.Sequence();

            // Move tất cả slots sang trái nửa cell
            for (int i = 0; i < slots.Count; i++)
            {
                Vector3 targetPos = slots[i].position + new Vector3(-cellSize / 2f, 0f, 0f);
                sequence.Join(slots[i].DOMove(targetPos, 0.3f));
            }

            for (int i = 0; i < tilesInShell.Count; i++)
            {
                Vector3 targetPos = slots[i].position + new Vector3(-cellSize / 2f, 0f, 0f);
                tilesInShell[i].transform.DOKill();
                sequence.Join(tilesInShell[i].transform.DOMove(targetPos, 0.35f));
            }

            // Sau khi move xong mới hiện slot mới
            sequence.OnComplete(() =>
            {
                slots[slots.Count - 1].GetComponent<SpriteRenderer>().enabled = true;
                additionSlot.SetActive(true);
                slots.Add(additionSlot.transform);
            });
        }

        public void MoveToReturnShell(List<TileCell> returnTiles, int tilesInReturnShell)
        {
            for (int i = 0; i < returnTiles.Count; i++)
            {
                var tile = returnTiles[i];

                tile.transform
                    .DOMove(returnSlots[i + tilesInReturnShell].position, 1f)
                    .SetEase(Ease.OutExpo);

                tilesInShell.Remove(tile);
                tile.IsClicked = false;
                tile.GetComponent<BoxCollider2D>().enabled = true;
            }

            RelayoutShell();
        }

        #endregion
    }
}