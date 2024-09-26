using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Bitsplash.DatePicker
{
    [ExecuteInEditMode]
    public partial class DatePickerContent : MonoBehaviour , IDatePickerSettingsItem
    {
        [SerializeField]
        [HideInInspector]
        private bool isOpen;

        const int RowCount = 6;
        const int ColumnCount = 7;

        [FormerlySerializedAs("FirstDayOfWeek")]
        [SerializeField]
        [Tooltip("the first day of the week for the content")]
        private DayOfWeek firstDayOfWeek = DayOfWeek.Sunday;

        [FormerlySerializedAs("RightToLeft")]
        [SerializeField]
        private bool rightToLeft = false;

        [FormerlySerializedAs("BottomToTop")]
        [SerializeField]
        private bool bottomToTop = false;

        [FormerlySerializedAs("CellPrefab")]
        [SerializeField]
        [Tooltip("drag a cell template here to use it with the datepicker")]
        private DatePickerCell cellPrefab = null;

        [FormerlySerializedAs("SelectionMode")]
        [SerializeField]
        [Tooltip("single,range and multiple selection types. ")]
        private SelectionType selectionMode;

        [FormerlySerializedAs("AllowEmptySelection")]
        [SerializeField]
        private bool allowEmptySelection = false;

        [SerializeField]
        private DateTime startDate = new DateTime(1960,1,1);

        [SerializeField]
        private DateTime endDate = new DateTime(2030, 12, 31);

        void ValidateYear()
        {
            if (endDate < startDate)
                endDate = startDate;
            mMonthFirst = new DateTime(mMonthFirst.Year, mMonthFirst.Month, 1);
            if(mMonthFirst > endDate)
            {
                mMonthFirst = new DateTime(endDate.Year, endDate.Month, 1);
            }
            if(mMonthFirst < startDate)
            {
                mMonthFirst = new DateTime(startDate.Year, startDate.Month, 1);
            }
        }
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value.Date;
                ValidateYear();
                Invalidate();
                OnSettingsChanged();
            }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value.Date;
                ValidateYear();
                Invalidate();
                OnSettingsChanged();
            }
        }

        /// <summary>
        /// the first day of the week for the date picker
        /// </summary>
        public DayOfWeek FirstDayOfWeek
        {
            get { return firstDayOfWeek; }
            set
            {
                firstDayOfWeek = value;
                OnSettingsChanged();
            }
        }

        /// <summary>
        /// set the date picker to right to left mode
        /// </summary>
        public bool RightToLeft
        {
            get { return rightToLeft; }
            set
            {
                rightToLeft = value;
                OnSettingsChanged();
            }
        }


        /// <summary>
        /// show days from bottom to top instead of top to bottom
        /// </summary>
        public bool BottomToTop
        {
            get { return bottomToTop; }
            set
            {
                bottomToTop = value;
                OnSettingsChanged();
            }
        }


        //public DatePickerCell CellPrefab
        //{
        //    get { return cellPrefab; }
        //    set
        //    {
        //        cellPrefab = value;
        //    }
        //}


            /// <summary>
            /// set the selection mode for the date picker. Single ,Range or Multiple
            /// </summary>
        public SelectionType SelectionMode
        {
            get { return selectionMode; }
            set
            {
                selectionMode = value;
                OnSettingsChanged();
            }
        }


        /// <summary>
        /// allows selection of the date picker to be empty
        /// </summary>
        public bool AllowEmptySelection
        {
            get { return allowEmptySelection; }
            set
            {
                allowEmptySelection = value;
                OnSettingsChanged();
            }
        }


        /// <summary>
        /// used for internal purpose
        /// </summary>
        public event Action SettingsChanged;
        /// <summary>
        /// currently the displayed month and year
        /// </summary>
        DateTime mMonthFirst = DateTime.Today;
        /// <summary>
        /// genearted cells
        /// </summary>
        DatePickerCell[] mCells;
        /// <summary>
        /// the selection collection object for the content
        /// </summary>
        DatePickerCollection mSelection = new DatePickerCollection();
        /// <summary>
        /// a date to cell map for quick lookup
        /// </summary>
        Dictionary<DateTime, DatePickerCell> mDateToCell = new Dictionary<DateTime, DatePickerCell>();

        /// <summary>
        /// an input delegation for the date picker
        /// </summary>
        DatePickerInput mDatePickerInput;

        /// <summary>
        /// true if the datepicker should be recreated
        /// </summary>
        bool mInvalidated = true;

        /// <summary>
        /// This event triggers when the use navigates the datepicker
        /// </summary>
        public UnityEvent OnDisplayChanged;
        /// <summary>
        /// this event triggers when the date selection has changed
        /// </summary>
        public UnityEvent OnSelectionChanged;

        /// <summary>
        /// the date picker selection collection. Use this object to change and query the current date selection
        /// </summary>
        public DatePickerCollection Selection { get { return mSelection; } }

        void EnsureInput()
        {
            mDatePickerInput = GetComponent<DatePickerInput>();
            if (mDatePickerInput == null)
                mDatePickerInput = gameObject.AddComponent<DatePickerStandardInput>();
        }

        /// <summary>
        /// the currently displayed date in the datepicker
        /// </summary>
        public DateTime DisplayDate { get { return mMonthFirst; } }

        /// <summary>
        /// sets the month and year being displayed in the date picker. 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        public void SetMonthAndYear(int year,int month)
        {
            FillCells(new DateTime(year, month, 1));
        }
        /// <summary>
        /// sets the year being displayed in the date picker
        /// </summary>
        /// <param name="year"></param>
        public void SetYear(int year)
        {
            FillCells(new DateTime(year, mMonthFirst.Month, 1));
        }
        /// <summary>
        /// sets the month being displayed in the date picker
        /// </summary>
        /// <param name="month"></param>
        public void SetMonth(int month)
        {
            FillCells(new DateTime(mMonthFirst.Year,month, 1));
        }
        /// <summary>
        /// used internally
        /// </summary>
        public string EditorTitle { get {return "Board"; } }

        /// <summary>
        /// used internally
        /// </summary>
        public int Order { get { return 0; } }

        /// <summary>
        /// advances the display by 1 year
        /// </summary>
        public void NextYear()
        {
            FillCells(mMonthFirst.AddYears(1));
        }
        void OnSettingsChanged()
        {
            if (SettingsChanged != null)
                SettingsChanged();
        }
        /// <summary>
        /// retracts the display by 1 year
        /// </summary>
        public void PrevYear()
        {
            FillCells(mMonthFirst.AddYears(-1));
        }
        /// <summary>
        /// advances the display by 1 month
        /// </summary>
        public void NextMonth()
        {
            FillCells(mMonthFirst.AddMonths(1));
        }
        /// <summary>
        /// retracts the display by 1 month
        /// </summary>
        public void PrevMonth()
        {
            FillCells(mMonthFirst.AddMonths(-1));
        }

        public virtual string DateToString(DateTime date)
        {
            return date.Day.ToString();
        }
        void GenerateCells()
        {
            Clear();
            if (cellPrefab == null)
                return;

            mCells = new DatePickerCell[((int)RowCount) * ((int)ColumnCount)];

            float ColumnSize = 1f / ColumnCount;
            float RowSize = 1f / RowCount;

            for(float i=0; i<RowCount; i++)
            {
                float startY = i / RowCount;
                if (BottomToTop == false)
                    startY = 1f - startY - RowSize;
                float endY = startY + RowSize;
                for (float j=0; j<ColumnCount; j++)
                {
                    float startX = j / ColumnCount;
                    if (RightToLeft)
                        startX = 1f - startX - ColumnSize;
                    float endX = startX + ColumnSize;

                    GameObject newObj = GameObject.Instantiate(cellPrefab.gameObject, transform);
                    CommonMethods.SafeDestroy(newObj.GetComponent<DatePickerCellTemplate>());
                    CommonMethods.HideObject(newObj);
                    newObj.name = String.Format("day_{0}_{1}", j, i);
                    newObj.SetActive(true);
                    CommonMethods.EnsureComponent<IDateTimeItem>(newObj);
                    var rect = newObj.GetComponent<RectTransform>();
                    rect.anchorMin = new Vector2(startX, startY);
                    rect.anchorMax = new Vector2(endX, endY);
                    rect.anchoredPosition = new Vector2(0f, 0f);
                    rect.sizeDelta = new Vector2(0f, 0f);
                    int childIndex = (int)(i * ColumnCount + j);
                    childIndex = (int)(i * ColumnCount + j);
                    mCells[childIndex] = newObj.GetComponent<DatePickerCell>();
                    var addon = CommonMethods.EnsureComponent<CellAddon>(newObj);
                    addon.SetParent(this, childIndex);

                }
            }

            FillCells(mMonthFirst);
        }
        DateTime MonthFromDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }
        DatePickerCell getCell(int day,int week)
        {
            return mCells[week * ColumnCount + day];
        }
        
        void FillCells(DateTime monthFirst)
        {
            monthFirst = monthFirst.Date;
            mMonthFirst = monthFirst;
            ValidateYear();
            if (mCells == null)
                return;

            monthFirst = mMonthFirst;
            int monthDayOfWeek = (int)monthFirst.DayOfWeek;
            int span = monthDayOfWeek - (int)FirstDayOfWeek;
            if (span < 0)
                span += 7;

            DateTime startFrom = (monthFirst - TimeSpan.FromDays(span)).Date;
            DateTime endIn = startFrom + TimeSpan.FromDays(RowCount * ColumnCount);
            DateTime monthLast = monthFirst + TimeSpan.FromDays(DateTime.DaysInMonth(monthFirst.Year, monthFirst.Month) - 1);
            DateTime current = startFrom;
            mDateToCell.Clear();
            for (int i=0; i<mCells.Length; i++)
            {
                mCells[i].DayValue = current;
                mCells[i].SetText(DateToString(current));
                bool cellenabled = true;
                if (current < monthFirst || current > monthLast || current < startDate || current > endDate)
                    cellenabled = false;
                mCells[i].SetInitialSettings(cellenabled, false);
                mDateToCell[current.Date] = mCells[i];
                current += TimeSpan.FromDays(1);
            }
            RefreshSelection();
            if (OnDisplayChanged != null)
                OnDisplayChanged.Invoke();
        }

        protected void Clear()
        {
            IDateTimeItem[] children = GetComponentsInChildren<IDateTimeItem>();
            for (int i = 0; i < children.Length; ++i)
            {
                if (children[i] != null)
                {
                    if (children[i].gameObject.GetComponentInParent<DatePickerContent>() != this)
                        continue;
                    if (children[i].gameObject != gameObject)
                        CommonMethods.SafeDestroy(children[i].gameObject);
                }
            }
        }

        public void Invalidate()
        {
            mInvalidated = true;
        }
        void HookEvents()
        {
            ((IDatePickerCollectionPrivate)mSelection).SelectionModified -= DatePicker_SelectionModified;
            ((IDatePickerCollectionPrivate)mSelection).SelectionModified += DatePicker_SelectionModified;
        }

        private void DatePicker_SelectionModified()
        {
            RaiseSelectionChanged();
        }

        public void Start()
        {
            HookEvents();
            EnsureInput();
            GenerateCells();
          //  if (AllowEmptySelection == false)
          //      SelectOne(DateTime.Today);
        }

        public void Update()
        {
            if(AllowEmptySelection != ((IDatePickerCollectionPrivate)mSelection).AllowEmpty)
                ((IDatePickerCollectionPrivate)mSelection).AllowEmpty = AllowEmptySelection;
            UpdateSelection();
            if(mInvalidated)
            {
                GenerateCells();
                mInvalidated = false;
            }
        }

        public void OnValidate()
        {
            ValidateYear();
            OnSettingsChanged();
            Invalidate();
        }

        /// <summary>
        /// retrives the underlaying gameobject specified by dateTime. If the dateTime is not currently displayed , null is returned
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public DatePickerCell GetCellObjectByDate(DateTime dateTime)
        {
            DatePickerCell res = null;
            if (mDateToCell.TryGetValue(dateTime.Date, out res))
                return res;
            return null;
        }
    }
}
