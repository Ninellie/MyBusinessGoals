using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsplash.DatePicker
{ 
    /// <summary>
    /// holds dates selected by the date picker
    /// </summary>
    public class DatePickerCollection : IDatePickerCollectionPrivate
    {
        bool mInvalid = false;
        HashSet<DateTime> mData = new HashSet<DateTime>();
        bool mChanged = false;
        bool mAllowEmpty = false;

        DateTime[] mItems;

        bool IDatePickerCollectionPrivate.Changed { get { return mChanged; } set { mChanged = value; } }

        bool IDatePickerCollectionPrivate.AllowEmpty { get { return mAllowEmpty; } set
            {
                mAllowEmpty = value;
                if(mAllowEmpty == false && mData.Count ==0)
                {
                    ValidateNonEmpty();
                    Invalidate();
                }
            }
        }

        event Action InnerSelectionModified;

        event Action IDatePickerCollectionPrivate.SelectionModified
        {
            add
            {
                InnerSelectionModified += value;
            }

            remove
            {
                InnerSelectionModified -= value;
            }
        }

        public int Count
        {
            get { return mData.Count; }
        }

        /// <summary>
        /// selects a range of dates , clearing all other dates
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void SelectRange(DateTime from,DateTime to)
        {
            if (CommonMethods.IsRangeSelected(from, to, mData))
                return;
            CommonMethods.SelectRange(from, to, mData);
            Invalidate();
        }

        void Invalidate()
        {
            mChanged = true;
            mInvalid = true;
            if (InnerSelectionModified != null)
                InnerSelectionModified();
        }

        /// <summary>
        /// returns true if a date is the only date selected
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool IsSingleDateSelected(DateTime date)
        {
            date = date.Date;
            if (mData.Contains(date) && mData.Count == 1)
                return true;
            return false;
        }

        /// <summary>
        /// selects one date clearing all other dates
        /// </summary>
        /// <param name="date"></param>
        public void SelectOne(DateTime date)
        {
            date = date.Date;
            if (mData.Contains(date) && mData.Count == 1)
                return;

            mData.Clear();
            Add(date);
        }

        void ValidateNonEmpty()
        {
            if (mAllowEmpty == false && mData.Count == 0)
                mData.Add(DateTime.Today.Date);
        }

        /// <summary>
        /// clears all selected dates. If the selection cannot be empty , DateTime.Today is set as the selection
        /// </summary>
        public void Clear()
        {
            if (mData.Count == 0)
                return;
            if (mAllowEmpty == false && IsSingleDateSelected(DateTime.Today))
                return;
            mData.Clear();
            ValidateNonEmpty();
            Invalidate();
        }

        /// <summary>
        /// appends all the items to the date collection
        /// </summary>
        /// <param name="range"></param>
        public void AddItems(HashSet<DateTime> range)
        {
            bool changed = false;
            foreach(DateTime d in range)
            {
                if (mData.Add(d))
                    changed = true;
            }
            if (changed)
                Invalidate();
        }

        /// <summary>
        /// adds a date into the date selection. 
        /// </summary>
        /// <param name="date"></param>
        public void Add(DateTime date)
        {
            if(mData.Add(date.Date))
                Invalidate();
        }

        /// <summary>
        /// returns true if the date collection contains the specified date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool Contains(DateTime date)
        {
            return mData.Contains(date.Date);
        }

        /// <summary>
        /// gets the date item at index 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DateTime GetItem(int index)
        {
            if(mInvalid || mItems == null)
            {
                mInvalid = false;
                mItems = mData.OrderBy(x => x).ToArray();
            }
            return mItems[index];
        }

        /// <summary>
        /// removes a date from the collection if it present. returns true if the date was present and was removed
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool Remove(DateTime date)
        {

            if (mData.Remove(date.Date))
            {
                ValidateNonEmpty();
                Invalidate();
                return true;
            }
            return false;
        }
    }
}
