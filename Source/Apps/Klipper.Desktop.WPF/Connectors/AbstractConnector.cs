using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Klipper.Desktop.WPF.Connectors
{
    public abstract class AbstractConnector : IConnector
    {
        protected Dictionary<string, IConnector> _childrenConnectors = new Dictionary<string, IConnector>();

        public AbstractConnector(IConnector parent, ContentControl ui)
        {
            Parent = parent;
            Ui = ui;
        }

        public IConnector Parent { get; private set; } = null;

        public string Tag { get; set; } = "";

        public ContentControl Ui { get; set; } = null;

        public bool Initialized { get; set; } = false;

        public List<string> Children
        {
            get
            {
                List<string> children = new List<string>();
                foreach(var k in _childrenConnectors.Keys)
                {
                    children.Add(k);
                }
                return children;
            }
        }

        public bool AddChild(string tag, IConnector connector)
        {
            if(_childrenConnectors.Keys.Contains(tag))
            {
                return false;
            }
            _childrenConnectors.Add(tag, connector);
            return true;
        }

        public IConnector Child(string tag)
        {
            if (_childrenConnectors.Keys.Contains(tag))
            {
                return _childrenConnectors[tag];
            }
            return null;
        }

        public virtual void Initialize()
        {
            if(Initialized)
            {
                return;
            }
            LoadViews();
            Initialized = true;
        }

        protected virtual void LoadViews()
        {

        }

    }
}


