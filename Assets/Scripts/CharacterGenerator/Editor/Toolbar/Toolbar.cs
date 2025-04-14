using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace ChaosHouse.CharacterGenerator
{
    public class Toolbar
    {
        private List<IToolbarItem<CharacterPart>> _list;
        private string[] itemsNames;
        private int _selectedTab = 0;
        public string[] ItemsNames => itemsNames;

        private Action _actionOnChanged;
        public void SetActionOnChanged(Action action)
        {
            _actionOnChanged = action;
        }

        public Toolbar()
        {
            _list = new List<IToolbarItem<CharacterPart>>();            
        }

        private void Subscribe()
        {
            for (int i = 0; i < _list.Count; i++) _list[i].onChanged += _actionOnChanged;
        }

        private void Unsubscribe()
        {
            for (int i = 0; i < _list.Count; i++) _list[i].onChanged -= _actionOnChanged;
        }

        public void Add<T>(IToolbarItem<CharacterPart> item) where T : CharacterPart
        {
            Unsubscribe();

            if (_list.Find(x => x.ItemName == item.ItemName) == null)
            {
                _list.Add(item);
                InitItemNames();
            }

            Subscribe();
        }
        public void InitItemNames()
        {
            itemsNames = new string[_list.Count];
            for (int i = 0; i < _list.Count; i++) itemsNames[i] = _list[i].ItemName;
        }

        public void Draw()
        {
            _selectedTab = GUILayout.Toolbar(_selectedTab, itemsNames);
            _list[_selectedTab].Draw();
        }

       
    }
}
