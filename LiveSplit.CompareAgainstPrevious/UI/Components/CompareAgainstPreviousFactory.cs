using System;
using LiveSplit.CompareAgainstPrevious;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiveSplit.Model;
using LiveSplit.UI.Components;


[assembly: ComponentFactory(typeof(CompareAgainstPreviousFactory))]
namespace LiveSplit.CompareAgainstPrevious
{
    class CompareAgainstPreviousFactory : IComponentFactory
    {
        public ComponentCategory Category
        {
            get
            {
                return ComponentCategory.Other;
            }
        }

        public string ComponentName
        {
            get
            {
                return "Compare Against Previous Run";
            }
        }

        public string Description
        {
            get
            {
                return String.Format("Updates the {0} to always have the previous runs splits", CompareAgainstPreviousComparisonGenerator.ComparisonName);
            }
        }

        public string UpdateName
        {
            get
            {
                return this.ComponentName;
            }
        }

        public string UpdateURL
        {
            get
            {
                return @"http://github.com/MikeMcMahon/LiveSplit.CompareAgainstPreviousComponent";
            }
        }

        public Version Version
        {
            get
            {
                return Version.Parse("1.0");
            }
        }

        public string XMLURL
        {
            get
            {
                return "";
            }
        }

        public IComponent Create(LiveSplitState state)
        {
            return new CompareAgainstPreviousComponent(state);
        }
    }
}
