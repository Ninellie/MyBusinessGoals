using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Bitsplash.DatePicker
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DatePickerContent : IDatePickerPrivate
    {
        DateTime? mSelectionFirst;
        DateTime? mDragFirst;
        DateTime? mDragTo;

        HashSet<DateTime> mDragSelectionRange = new HashSet<DateTime>();
        bool mDragSelectionChanged = false;


        Dictionary<DateTime, Color> mMarkerColors = new Dictionary<DateTime, Color>();
        public void SetAllMarkerColors(Dictionary<DateTime, Color> markers)
        {
            
            mMarkerColors.Clear();
            foreach (var pair in markers)
                mMarkerColors.Add(pair.Key.Date, pair.Value);
            RefreshSelection();
        }
        public void SetMarkerColor(DateTime date, Color color)
        {
            mMarkerColors[date.Date] = color;
            RefreshSelection();
        }
        public void ClearMarker(DateTime date)
        {
            mMarkerColors.Remove(date.Date);
            RefreshSelection();
        }
        public void ClearMarkerColor()
        {
            mMarkerColors.Clear();
            RefreshSelection();
            
        }
        public void SelectOne(DateTime date)
        {
            mSelection.SelectOne(date);
            mSelectionFirst = date;
        }

        void ToogleOne(DateTime date)
        {
            if (mSelection.IsSingleDateSelected(date) && AllowEmptySelection)
            {
                mSelection.Clear();
                mSelectionFirst = null;
            }
            else
                SelectOne(date);   
        }
        void UpdateSelection()
        {
            if(mDragSelectionChanged || ((IDatePickerCollectionPrivate) mSelection).Changed)
            {
                ((IDatePickerCollectionPrivate)mSelection).Changed = false;
                mDragSelectionChanged = false;
                RefreshSelection();
            }
        }
        void RefreshSelection()
        {
            for (int i = 0; i < mCells.Length; i++)
            {
                var date = mCells[i].DayValue.Date;
                bool withinMonth = date.Month == DisplayDate.Month && date.Year == DisplayDate.Year;
                Color markerColor;
                if (mMarkerColors.TryGetValue(date, out markerColor) == false)
                    markerColor = new Color(0f, 0f, 0f, 0f);
                if (mCells[i].MarkerColor != markerColor)
                    mCells[i].MarkerColor = markerColor;
                if ((mSelection.Contains(date) || mDragSelectionRange.Contains(date)) && withinMonth)
                {
                    if (mCells[i].CellSelected == false)
                        mCells[i].CellSelected = true;
                }
                else
                {
                    if(mCells[i].CellSelected == true)
                        mCells[i].CellSelected = false;
                }
            }
        }

        void LimitRangeToMonth(HashSet<DateTime> selection,DateTime month)
        {
            selection.RemoveWhere((x) => x.Month != month.Month || x.Year != month.Year);
        }


        void SelectRange(DateTime from,DateTime to)
        {
            mSelection.SelectRange(from, to);
        }

        void ToogleMultiple(DateTime date)
        {
            if (mSelection.Contains(date))
            {
                if (mSelection.Count > 1 || AllowEmptySelection)
                    mSelection.Remove(date);
            }
            else
            {
                mSelection.Add(date);
            }
        }

        void ConnectSelection(DateTime date)
        {
            date = date.Date;
            if (mSelection.Contains(date)) // already within the selection
                return;
            if(mSelection.Count == 0 || mSelectionFirst.HasValue == false)
            {
                SelectOne(date);
                return;
            }
            SelectRange(mSelectionFirst.Value, date);
        }

        void ProcessRangeClick(DatePickerCell cell, int cellChildIndex)
        {
            if (mDatePickerInput.MultipleSelectionValue == MultipleSelectionInputValue.Append)
                ConnectSelection(cell.DayValue);
            else if (mDatePickerInput.MultipleSelectionValue == MultipleSelectionInputValue.Singular)
                ToogleOne(cell.DayValue);
        }

        void ProcessMultipleClick(DatePickerCell cell, int cellChildIndex)
        {
            if (mDatePickerInput.MultipleSelectionValue == MultipleSelectionInputValue.Append)
                ToogleMultiple(cell.DayValue);
            else if (mDatePickerInput.MultipleSelectionValue == MultipleSelectionInputValue.Singular)
                ToogleOne(cell.DayValue);
        }
        void ProcessSelectionClick(DatePickerCell cell,int cellChildIndex)
        {
            switch (SelectionMode)
            {
                case SelectionType.Single:
                    ToogleOne(cell.DayValue);
                    break;
                case SelectionType.Range:
                    ProcessRangeClick(cell, cellChildIndex);
                    break;
                case SelectionType.Multiple:
                    ProcessMultipleClick(cell, cellChildIndex);
                    break;
            }
        }

        protected virtual void OnCellClick(DatePickerCell cell, int cellChildIndex)
        {
            if (cell.CellEnabled == false)
                return;
            if (mDragFirst.HasValue || mDragTo.HasValue)
                return;
            ProcessSelectionClick(cell, cellChildIndex);
        }

        void RaiseSelectionChanged()
        {
            if (OnSelectionChanged != null)
                OnSelectionChanged.Invoke();
        }
        void IDatePickerPrivate.RaiseClick(int childIndex)
        {
            OnCellClick(mCells[childIndex], childIndex);
        }

        void IDatePickerPrivate.RaiseStartSelection(int childIndex)
        {
            if (SelectionMode == SelectionType.Single)
                return;
            DateTime dayValue = mCells[childIndex].DayValue;
            if (SelectionMode == SelectionType.Range || (SelectionMode == SelectionType.Multiple && mDatePickerInput.MultipleSelectionValue == MultipleSelectionInputValue.Singular))
                mSelection.SelectOne(dayValue);
            mDragFirst = dayValue;
            if (mDragTo.HasValue && mDragFirst.HasValue)
            {
                CommonMethods.SelectRange(mDragTo.Value, mDragFirst.Value, mDragSelectionRange);
            }
            else
            {
                mDragSelectionRange.Clear();
                mDragSelectionRange.Add(dayValue);
            }
            mSelectionFirst = null;
            LimitRangeToMonth(mDragSelectionRange, mMonthFirst);
            mDragSelectionChanged = true;
        }

        void IDatePickerPrivate.EndSelection()
        {
            if (SelectionMode == SelectionType.Single)
                return;
            if (mDragTo.HasValue && mDragFirst.HasValue)
            {
                CommonMethods.SelectRange(mDragTo.Value, mDragFirst.Value, mDragSelectionRange);
                LimitRangeToMonth(mDragSelectionRange, mMonthFirst);
                mSelection.AddItems(mDragSelectionRange);
            }
            mDragSelectionRange.Clear();
            mDragSelectionChanged = true;
            mDragTo = null;
            mDragFirst = null;
        }

        void IDatePickerPrivate.RaiseSelectionEnter(int childIndex, int fromChildIndex)
        {
            if (SelectionMode == SelectionType.Single)
                return;
            mSelectionFirst = null;
            mDragTo = mCells[childIndex].DayValue;
            if (mDragTo.HasValue && mDragFirst.HasValue)
            {
                CommonMethods.SelectRange(mDragTo.Value, mDragFirst.Value, mDragSelectionRange);
                LimitRangeToMonth(mDragSelectionRange, mMonthFirst);
            }
            mDragSelectionChanged = true;
        }

        void IDatePickerPrivate.RaiseSelectionExit(int childIndex, int fromChildIndex)
        {
            if (SelectionMode == SelectionType.Single)
                return;
//            if (mDragTo.HasValue && mDragTo.Value == mCells[childIndex].DayValue)
//                mDragTo = null;
        }
    }
}
