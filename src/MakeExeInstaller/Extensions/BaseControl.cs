using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MakeExeInstaller.Extensions
{
    /// <summary>
    /// base control
    /// </summary>
    public abstract class BaseControl : Control
    {
        private readonly List<object> _logicalChildren = new List<object>();

        /// <summary>
        /// default constructor
        /// </summary>
        public BaseControl()
        {
            UseLayoutRounding = true;
        }

        #region Methods
        /// <summary>
        /// logical children
        /// </summary>
        protected override IEnumerator LogicalChildren => _logicalChildren.GetEnumerator();

        /// <summary>
        /// add logical child
        /// </summary>
        /// <param name="child">child</param>
        protected new void AddLogicalChild(object child)
        {
            if (child == null) return;
            base.AddLogicalChild(child);
            _logicalChildren.Add(child);
        }

        /// <summary>
        /// remove logical child
        /// </summary>
        /// <param name="child">child</param>
        protected new void RemoveLogicalChild(object child)
        {
            if (child == null) return;
            base.RemoveLogicalChild(child);
            _logicalChildren.Remove(child);
        }

        /// <summary>
        /// clear logical children
        /// </summary>
        protected void ClearLogicalChild()
        {
            _logicalChildren.ForEach(RemoveLogicalChild);
        }

        /// <summary>
        /// when property changed,add or remove as logical child
        /// </summary>
        /// <param name="dependency">dependency object</param>
        /// <param name="args">dependency property changed event args</param>
        protected static void SetLoigcalTree(DependencyObject dependency, DependencyPropertyChangedEventArgs args)
        {
            if (dependency is BaseControl control)
            {
                if (args.OldValue != null) control.RemoveLogicalChild(args.OldValue);
                if (args.NewValue != null) control.AddLogicalChild(args.NewValue);
            }
        }

        /// <summary>
        /// get element from template by name
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="name">element name</param>
        /// <param name="isRequired">if true and not found element with the name or not the type of T,exception occured</param>
        /// <returns>element of type T</returns>
        protected T GetTemplateChild<T>(string name, bool isRequired = false) where T : class
        {
            var result = GetTemplateChild(name) as T;
            if (isRequired && result == null) throw new Exception($"PART with name '{name}' required and must be type '{typeof(T).FullName}'");
            return result;
        }
        #endregion
    }
}

