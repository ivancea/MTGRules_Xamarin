using MTGRules.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace MTGRules
{
    public class RuleHyperlinkConverter : IValueConverter
    {
        private static readonly Regex regex = new Regex(@"(\d{3}(?:\.\d+[a-z]?)?)", RegexOptions.IgnoreCase);

        private static readonly Command<Span> ruleTapCommand;

        static RuleHyperlinkConverter()
        {
            ruleTapCommand = new Command<Span>((spanObject) => {
                if (MainPage.ActualInstance != null)
                {
                    MainPage.ActualInstance.OnHyperlinkRuleClick(spanObject.Text);
                }
            });
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = value as string;

            var formattedString = new FormattedString();
            
            var str = regex.Split(text);

            for (int i = 0; i < str.Length; i++)
            {
                if (i % 2 == 0)
                {
                    formattedString.Spans.Add(new Span { Text = str[i] });
                }
                else
                {
                    Span hyperlinkSpan = new Span { Text = str[i] };

                    hyperlinkSpan.TextColor = Color.Accent;
                    //hyperlinkSpan.TextDecorations = TextDecorations.Underline;

                    TapGestureRecognizer ruleTapGestureRecognizer = new TapGestureRecognizer
                    {
                        Command = ruleTapCommand,
                        CommandParameter = hyperlinkSpan
                    };

                    hyperlinkSpan.GestureRecognizers.Add(ruleTapGestureRecognizer);

                    formattedString.Spans.Add(hyperlinkSpan);
                }
            }

            return formattedString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
