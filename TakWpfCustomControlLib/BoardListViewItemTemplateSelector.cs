using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TakLib;

namespace TakWpfControls
{
    public class BoardListViewItemTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement elemnt = container as FrameworkElement;
            Board board = item as Board;
            if (board != null)
            {
                return elemnt.FindResource("BoardItemTemplate") as DataTemplate;
            }
            else
            {
                return elemnt.FindResource("BoardAnalysisTemplate") as DataTemplate;
            }
        }
    }
}
