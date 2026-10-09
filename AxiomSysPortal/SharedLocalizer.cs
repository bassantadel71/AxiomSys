using System.Globalization;

namespace AxiomSysPortal
{
	public class SharedLocalizer
	{
		private readonly Dictionary<string, string[]> _texts = new Dictionary<string, string[]>();

		public SharedLocalizer()
		{
			// Keys given in the pack
			_texts.Add("32", new string[] { "Add", "إضافة" });
			_texts.Add("5583", new string[] { "Edit", "تعديل" });
			_texts.Add("18", new string[] { "Delete", "حذف" });
			_texts.Add("3", new string[] { "Code", "الكود" });
			_texts.Add("5", new string[] { "Arabic name", "الاسم بالعربي" });
			_texts.Add("4", new string[] { "English name", "الاسم بالإنجليزي" });

			// Extra keys I made up (the pack does not list them)
			_texts.Add("9001", new string[] { "Days", "الأيام" });
			_texts.Add("9002", new string[] { "Active", "فعال" });
			_texts.Add("9003", new string[] { "Save", "حفظ" });
			_texts.Add("9004", new string[] { "Cancel", "إلغاء" });
			_texts.Add("9005", new string[] { "Delivery Terms", "شروط التسليم" });
			_texts.Add("9006", new string[] { "Search...", "بحث..." });
			_texts.Add("9007", new string[] { "Are you sure?", "هل أنت متأكد؟" });
			_texts.Add("9008", new string[] { "This record will be deleted.", "سيتم حذف هذا السجل." });
			_texts.Add("9009", new string[] { "Actions", "الإجراءات" });
			_texts.Add("9010", new string[] { "Yes, delete", "نعم، احذف" });
			_texts.Add("9011", new string[] { "Yes", "نعم" });
			_texts.Add("9012", new string[] { "No", "لا" });
			_texts.Add("9013", new string[] { "Please enter the name in Arabic.", "الرجاء إدخال الاسم باللغة العربية." });
			_texts.Add("9014", new string[] { "Please enter the name in English.", "الرجاء إدخال الاسم باللغة الإنجليزية." });
			_texts.Add("9015", new string[] { "Could not load the record.", "تعذر تحميل السجل." });
			_texts.Add("9016", new string[] { "Show _MENU_ entries", "عرض _MENU_ سجل" });
			_texts.Add("9017", new string[] { "Showing _START_ to _END_ of _TOTAL_ entries", "عرض _START_ إلى _END_ من أصل _TOTAL_ سجل" });
			_texts.Add("9018", new string[] { "No entries to show", "لا توجد سجلات للعرض" });
			_texts.Add("9019", new string[] { "No matching records found", "لا توجد سجلات مطابقة" });
			_texts.Add("9020", new string[] { "Next", "التالي" });
			_texts.Add("9021", new string[] { "Previous", "السابق" });
			_texts.Add("9022", new string[] { "(filtered from _MAX_ total entries)", "(تمت التصفية من أصل _MAX_ سجل)" });
		}

		public string this[string key]
		{
			get
			{
				if (!_texts.ContainsKey(key))
				{
					return key;
				}

				string[] pair = _texts[key];
				if (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ar")
				{
					return pair[1];
				}
				return pair[0];
			}
		}
	}
}
