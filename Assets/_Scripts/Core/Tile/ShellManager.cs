using System.Collections.Generic;
using System.Linq;
using _Scripts.Utils.Event_Bus;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Core.Tile
{
    public class ShellManager : Singleton<ShellManager>
    {
        [Header("Shell")] [SerializeField] private GameObject shell;
        [SerializeField] private GameObject additionSlot;
        public List<Transform> slots = new List<Transform>();
        public List<TileCell> tilesInShell = new List<TileCell>();

        [Header("Return Slots")] [SerializeField]
        private GameObject returnShell;

        public List<Transform> returnSlots = new List<Transform>();

        private Dictionary<int, List<int>>
            _similarTiles = new Dictionary<int, List<int>>(); // List(nums of tiles, first Position)

        private List<TileCell> _destroyingCellS = new List<TileCell>();
        bool _isDestroying = false;
        int _fullSlots = 0;
        int _filledSlots = 0;

        private void OnEnable() => EventBus.Subscribe<TileClickEvent>(AddTile);
        private void OnDisable() => EventBus.Unsubscribe<TileClickEvent>(AddTile);

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        private void AddTile(TileClickEvent evt)
        {
            TileCell tile = evt.TileCell;
            _fullSlots = _isDestroying ? slots.Count : slots.Count - 1;
            _filledSlots = tilesInShell.Count;
            if (_filledSlots >= _fullSlots)
                return;

            int index;
            InsertIntoShell(tile, out index);

            MoveRightOtherTiles(index, out var sequence);

            sequence.Join(tile.transform.DOMove(slots[index].position, 0.35f).SetEase(Ease.OutExpo));

            sequence.OnComplete(() => { CheckMatching(tile.ID); });
        }

        private void MoveRightOtherTiles(int index, out Sequence sequence)
        {
            bool haveSameTile = false;
            sequence = DOTween.Sequence();
            for (int i = 0; i < tilesInShell.Count; i++)
            {
                if (i <= index) continue;

                sequence.Join(tilesInShell[i].transform.DOMove(slots[i].position, 0.35f).SetEase(Ease.OutExpo));
                if (_similarTiles[tilesInShell[i].ID][0] == 2 && haveSameTile)
                {
                    haveSameTile = false;
                    continue;
                }

                _similarTiles[tilesInShell[i].ID][1]++;
                haveSameTile = true;
            }
        }

        private void InsertIntoShell(TileCell tile, out int index)
        {
            int id = tile.ID;
            if (!_similarTiles.ContainsKey(id))
            {
                if (tilesInShell.Count == 0)
                {
                    index = 0;
                }
                else index = _filledSlots;

                _similarTiles[id] = new List<int>()
                {
                    1,
                    index
                };
            }
            else
            {
                index = _similarTiles[id][0] + _similarTiles[id][1];

                _similarTiles[id][0]++;

            }

            tilesInShell.Insert(index, tile);
        }

        void CheckMatching(int id)
        {
            if (!_similarTiles.TryGetValue(id, out var similarID)) return;
            if (similarID[0] < 3) return;
            _isDestroying = true;
            int indexOfFirstOne = similarID[1];
      
            RemoveMatching(indexOfFirstOne, id);
        }

        void RemoveMatching(int index, int id)
        {
            for (int i = 0; i < 3; i++)
            {
                _destroyingCellS.Add(tilesInShell[index + i]);
            }



            Sequence sq = DOTween.Sequence();

            foreach (var cell in _destroyingCellS)
            {
                cell.transform.DOKill();

                sq.Join(
                    cell.transform.DOPunchScale(
                        new Vector3(0.5f, 0.5f, 0f),
                        0.5f,
                        3,
                        0.8f
                    )
                );
            }

            sq.AppendCallback(() =>
            {
                foreach (var cell in _destroyingCellS)
                {
                    tilesInShell.Remove(cell);
                    Destroy(cell.gameObject);
                }

                _destroyingCellS.Clear();
                _similarTiles.Remove(id);
            });

            sq.OnComplete(() =>
            {
                RebuildDictionary();
                _isDestroying = false;
                MoveLeftOtherTiles();
            });
        }

        void MoveLeftOtherTiles()
        {
            Debug.Log($"6.. MoveLeftOtherTiles");
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(0.1f);
            for (int i = 0; i < tilesInShell.Count; i++)
            {
                sequence.Join(
                    tilesInShell[i].transform.DOMove(slots[i].position, 0.35f).SetEase(Ease.OutExpo));
            }
        }

        void RebuildDictionary()
        {
            _similarTiles.Clear();
            for (int i = 0; i < tilesInShell.Count; i++)
            {
                int id = tilesInShell[i].ID;
                if (!_similarTiles.TryGetValue(id, out var similarTile))
                {
                    _similarTiles[id] = new List<int>()
                    {
                        1,
                        i
                    };
                }
                else
                {
                    similarTile[0]++;
                }
            }

        }

        public void AddSlot()
        {
            slots[slots.Count - 1].GetComponent<SpriteRenderer>().enabled = true;
            additionSlot.SetActive(true);
            slots.Add(additionSlot.transform);
        }

        public void Init()
        {
            if (additionSlot.activeInHierarchy)
                additionSlot.SetActive(false);
            slots = shell.GetComponentsInChildren<Transform>().Where(t => t != shell.transform).ToList();
            slots[slots.Count - 1].GetComponent<SpriteRenderer>().enabled = false;

            returnSlots = returnShell.GetComponentsInChildren<Transform>().Where(t => t != returnShell.transform).ToList();
        }

        public void MoveToReturnShell(List<TileCell> returnTiles, int tilesInReturnShell)
        {
            for (int i = 0; i < returnTiles.Count; i++)
            {
                var tile = returnTiles[i];

                Debug.Log($"{tile.name} + {tile.ID}");

                tile.transform
                    .DOMove(returnSlots[i + tilesInReturnShell].position, 1f)
                    .SetEase(Ease.OutExpo);

                tilesInShell.Remove(tile);
                _filledSlots = tilesInShell.Count;

                UpdateSimilarTiles(tile);
                tile.IsClicked = false;
                tile.GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        private void UpdateSimilarTiles(TileCell tile)
        {
            int tilesNum = --_similarTiles[tile.ID][0];
            if (tilesNum == 0)
            {
                _similarTiles.Remove(tile.ID);
            }
        }
    }
}