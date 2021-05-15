using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using KRV.LawnPro.Mobile.Custom;
using KRV.LawnPro.Mobile.Droid.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MyDatePicker), typeof(MyDatePickerRenderer))]

namespace KRV.LawnPro.Mobile.Droid.Custom
{
    public class MyDatePickerRenderer : DatePickerRenderer
    {
        public MyDatePickerRenderer(Context context) : base(context)
        {

        }


        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
        {
            base.OnElementChanged(e);

            //Disposing
            if (e.OldElement != null)
            {
                _element = null;
            }

            //Creating
            if (e.NewElement != null)
            {
                _element = e.NewElement;
            }
        }

        protected Xamarin.Forms.DatePicker _element;

        protected override DatePickerDialog CreateDatePickerDialog(int year, int month, int day)
        {
            var dialog = new DatePickerDialog(Context, (o, e) =>
            {
                _element.Date = e.Date;
                ((IElementController)_element).SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
            }, year, month, day);

            dialog.SetButton((int)DialogButtonType.Positive, Context.Resources.GetString(global::Android.Resource.String.Ok), OnOk);
            dialog.SetButton((int)DialogButtonType.Negative, Context.Resources.GetString(global::Android.Resource.String.Cancel), OnCancel);

            return dialog;
        }

        private void OnCancel(object sender, DialogClickEventArgs e)
        {
            _element.Date = DateTime.MinValue;
            _element.Unfocus();
        }
        private void OnOk(object sender, DialogClickEventArgs e)
        {
            //need to set date from native control manually now
            _element.Date = ((DatePickerDialog)sender).DatePicker.DateTime;
            _element.Unfocus();
        }
    }
}