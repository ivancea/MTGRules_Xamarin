using MTGRules.Interfaces;
using MTGRules.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MTGRules.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private class ActivityIndicatorViewer : IDisposable
        {
            public static MainPage page;

            private static int counter = 0;

            public static ActivityIndicatorViewer Show()
            {
                return new ActivityIndicatorViewer();
            }

            private ActivityIndicatorViewer()
            {
                if (counter++ == 0)
                {
                    page.content.IsVisible = false;
                    page.activityIndicator.IsVisible = true;
                }
            }

            public void Dispose()
            {
                counter -= 1;
                if (counter < 0)
                    counter = 0;
                if (counter == 0)
                {
                    page.activityIndicator.IsVisible = false;
                    page.content.IsVisible = true;
                }
            }
        }

        public static MainPage ActualInstance;

        private List<Rule> actualRules;

        private readonly List<HistoryItem> history = new List<HistoryItem>();
        private HistoryItem actualHistoryItem;

        private bool useLightTheme;
        private int actualRulesIndex;

        public Command HomeCommand { get; private set; }
        public Command ClearCacheCommand { get; private set; }

        private bool existsCache = true;

        public MainPage()
        {
            InitializeComponent();

            /*useLightTheme = (bool)Application.Current.Properties["useLightTheme"];
            if (useLightTheme)
            {
                changeThemeButton.Label = MainResources.useDarkTheme;
            }
            else
            {
                changeThemeButton.Label = MainResources.useLightTheme;
            }*/

            HomeCommand = new Command(() => {
                ShowByNumber();
            }, () => {
                return history.Count >= 1 && (actualHistoryItem == null || !actualHistoryItem.Type.Equals(HistoryType.Number) || !actualHistoryItem.Value.Equals(0));
            });

            ClearCacheCommand = new Command(() => {
                ClearCache();

                existsCache = false;

                ClearCacheCommand.ChangeCanExecute();
            }, () => {
                return existsCache;
            });

            ActualInstance = this;

            BindingContext = this;
        }

        private async void OnAppear(object sender, EventArgs e)
        {
            ActivityIndicatorViewer.page = this;

            if (actualRules == null)
            {
                using (ActivityIndicatorViewer.Show())
                {
                    if (!await LoadDataAsync(RulesVersionsService.RulesSources.Count - 1))
                    {
                        await DisplayAlert(MainResources.error, MainResources.errorLoadingLastRules, MainResources.ok);// MainResources.errorLoadingLastRules);
                    }
                }
            }
        }

        private void LogEvent(EventType eventType)
        {
            DependencyService.Get<IEventLogger>()?.Log(eventType);
        }

        public void OnHyperlinkRuleClick(string text)
        {
            ShowByKey(text);
        }

        private void OnRandomRuleButtonClick(object sender, EventArgs e)
        {
            ShowRandomRule();
        }

        private void OnDonateButtonClick(object sender, EventArgs e)
        {
            /*using (ActivityIndicatorViewer.Show())
            {
                MessageDialog dialog = new MessageDialog("");
                try
                {
                    ListingInformation info = await CurrentApp.LoadListingInformationAsync();
                    ProductListing donation;
                    if (info.ProductListings.TryGetValue("donation", out donation))
                    {
                        if (await fulfillDonation())
                        {
                            PurchaseResults purchaseResult =
                                await CurrentApp.RequestProductPurchaseAsync(donation.ProductId);
                            if (purchaseResult.Status == ProductPurchaseStatus.Succeeded)
                            {
                                dialog.Content = MainResources.thanksForDonation +
                                                 "\n" + MainResources.youCanRate;
                            }
                            else if (purchaseResult.Status == ProductPurchaseStatus.NotPurchased)
                            {
                                dialog.Content =
                                    MainResources.anywayThanksForUsingApp +
                                    "\n" + MainResources.youCanRate;
                            }
                            else
                            {
                                dialog.Content = MainResources.errorOcurred +
                                                 "\n" +
                                                 MainResources.thanksAnywayTryAgainOrRate;
                            }
                        }
                        else
                        {
                            dialog.Content =
                                MainResources.errorOcurredDonationPendingOrServerError +
                                "\n" + MainResources.thanksAnywayTryAgainOrRate;
                        }
                    }
                    else
                    {
                        dialog.Content = MainResources.noActiveDonations +
                                         "\n" + MainResources.youCanRate;
                    }
                }
                catch
                {
                    dialog.Content = MainResources.errorOcurred +
                                     "\n" + MainResources.thanksAnywayTryAgainOrRate;
                }

                await dialog.ShowAsync();
            }*/
        }

        /*private Task<bool> FulfillDonation()
        {
            var unfulfilledConsumables = await CurrentApp.GetUnfulfilledConsumablesAsync();
            foreach (UnfulfilledConsumable consumable in unfulfilledConsumables)
            {
                if (consumable.ProductId == "donation")
                {
                    FulfillmentResult fulfillmentResult = await CurrentApp.ReportConsumableFulfillmentAsync(consumable.ProductId, consumable.TransactionId);
                    switch (fulfillmentResult)
                    {
                        case FulfillmentResult.Succeeded:
                        case FulfillmentResult.NothingToFulfill:
                        case FulfillmentResult.PurchaseReverted:
                            return true;
                    }
                    return false;
                }
            }
            return true;
        }*/

        private void OnChangeThemeButtonClick(object sender, EventArgs args)
        {
            /*useLightTheme = !useLightTheme;
            Application.Current.Properties["useLightTheme"] = useLightTheme;
            await Application.Current.SavePropertiesAsync();

            if (useLightTheme)
            {
                changeThemeButton.Label = MainResources.useDarkTheme;
            }
            else
            {
                changeThemeButton.Label = MainResources.useLightTheme;
            }

            MessageDialog dialog = new MessageDialog(MainResources.restartToApply);
            await dialog.ShowAsync();*/
        }

        protected override bool OnBackButtonPressed()
        {
            return PopHistoryItem();
        }

        private void OnBackButtonClicked(object sender, EventArgs args)
        {
            PopHistoryItem();
        }

        private void OnListItemTapped(object sender, ItemTappedEventArgs args)
        {
            Rule rule = (Rule) args.Item;

            if (rule.Title.Length > 0)
            {
                if (rule.Title[0] >= '1' && rule.Title[0] <= '9')
                {
                    int n = rule.Title.IndexOf('.');
                    if (n >= 0 && rule.Title.Length - 1 == n)
                    {
                        if (int.TryParse(rule.Title.Substring(0, n), out n))
                        {
                            ShowByNumber((short)n);
                        }
                    }
                }
                else if (rule.Title == "Glosary")
                {
                    ShowByNumber(10);
                }
            }
        }

        private void OnTextBoxCompleted(object sender, EventArgs args)
        {
            Focus();
            Search(searchTextBox.Text);
        }

        private void ClearCache()
        {
            foreach (RulesSource source in RulesVersionsService.RulesSources)
            {
                try
                {
                    if (File.Exists(source.FileName))
                    {
                        File.Delete(source.FileName);
                    }
                }
                catch
                {
                    // ignored
                }
            }
        }

        private async void OnChangeRulesButtonClick(object sender, EventArgs args)
        {
            using (ActivityIndicatorViewer.Show())
            {
                int n = await ShowRulesListPicker(MainResources.selectTheRules);

                if (n >= 0 && n < RulesVersionsService.RulesSources.Count)
                {
                    if (!await LoadDataAsync(n))
                    {
                        await DisplayAlert(MainResources.error, MainResources.errorLoadingRules, MainResources.ok);
                    }
                }
            }
        }

        private void OnAboutButtonClick(object sender, EventArgs args)
        {
            Navigation.PushAsync(new AboutPage());
        }

        private async void OnExperimentalButtonClick(object sender, EventArgs args)
        {
            using (ActivityIndicatorViewer.Show())
            {
                int n1 = await ShowRulesListPicker(MainResources.selectOldRules);
                if (n1 < 0 || n1 >= RulesVersionsService.RulesSources.Count)
                    return;

                int n2 = await ShowRulesListPicker(MainResources.selectNewRules);
                if (n2 < 0 || n2 >= RulesVersionsService.RulesSources.Count)
                    return;

                List<Rule> from = await GetRulesAsync(RulesVersionsService.RulesSources[n1]),
                           to = await GetRulesAsync(RulesVersionsService.RulesSources[n2]);
                if (from == null || to == null)
                {
                    await DisplayAlert(MainResources.error, MainResources.errorLoadingRules, MainResources.ok);
                    return;
                }

                List<Rule> li = new List<Rule>();

                Rule rule;

                foreach (Rule r1 in from)
                {
                    rule = Compare(r1, FindRule(to, r1.Title));
                    if (rule != null)
                        li.Add(rule);
                    foreach (Rule r2 in r1.SubRules)
                    {
                        rule = Compare(r2, FindRule(to, r2.Title));
                        if (rule != null)
                            li.Add(rule);
                        foreach (Rule r3 in r2.SubRules)
                        {
                            rule = Compare(r3, FindRule(to, r3.Title));
                            if (rule != null)
                                li.Add(rule);
                        }
                    }
                }

                foreach (Rule r1 in to)
                {
                    rule = FindRule(from, r1.Title);
                    if (rule == null)
                        li.Add(new Rule("(+) " + r1.Title, r1.Text));
                    foreach (Rule r2 in r1.SubRules)
                    {
                        rule = FindRule(from, r2.Title);
                        if (rule == null)
                            li.Add(new Rule("(+) " + r2.Title, r2.Text));
                        foreach (Rule r3 in r2.SubRules)
                        {
                            rule = FindRule(from, r3.Title);
                            if (rule == null)
                                li.Add(new Rule("(+) " + r3.Title, r3.Text));
                        }
                    }
                }

                list.ItemsSource = li;

                PushHistoryItem(null);

                LogEvent(EventType.CompareRules);
            }
        }

        private async Task<int> ShowRulesListPicker(string title)
        {
            string selectedRules = await DisplayActionSheet(title, MainResources.cancel, null,
                RulesVersionsService.RulesSources.Select(e => e.Date.ToShortDateString()).Reverse().ToArray());

            return RulesVersionsService.RulesSources.FindIndex(r => selectedRules.StartsWith(r.Date.ToShortDateString()));
        }

        private static Rule Compare(Rule from, Rule to)
        {
            if (to == null)
                return new Rule("(-) " + from.Title, from.Text);
            if (from == null)
                return new Rule("(+) " + to.Title, to.Text);
            if (from.Title != to.Title)
                return null;
            if (from.Text != to.Text)
                return new Rule("(M) " + from.Title, from.Text + "\n\n " +
                                                     MainResources.compareChangedTo +
                                                     " \n\n" + to.Text);
            return null;
        }

        private static Rule FindRule(List<Rule> rules, string title)
        {
            foreach (Rule r1 in rules)
            {
                if (r1.Title == title)
                    return r1;
                foreach (Rule r2 in r1.SubRules)
                {
                    if (r2.Title == title)
                        return r2;
                    foreach (Rule r3 in r2.SubRules)
                    {
                        if (r3.Title == title)
                            return r1;
                    }
                }
            }
            return null;
        }

        private void OnSearch(object sender, EventArgs args)
        {
            Search(searchTextBox.Text);
        }

        private void SpeechText(string text)
        {
            DependencyService.Get<ITextToSpeech>().Speak(text);

            LogEvent(EventType.TextToSpeech);
        }

        private void OnCopyToClipboard(object sender, EventArgs args)
        {
            Rule rule = (Rule)((MenuItem)sender).BindingContext;

            Clipboard.SetText(rule.Title + ": " + rule.Text);
        }

        private void OnReadText(object sender, EventArgs args)
        {
            Rule rule = (Rule)((MenuItem)sender).BindingContext;

            string text = rule.Text;

            if (text.Length > 0)
            {
                if (text[0] >= '1' && text[0] <= '9')
                {
                    text = text.Substring(text.IndexOf(' ') + 1);
                }

                SpeechText(text);
            }
        }

        private void ShowRandomRule(int? seed = null, bool addToHistory = true)
        {
            if (actualRules == null)
                return;
            if (seed == null)
                seed = DateTime.Now.Millisecond;
            Random random = new Random(seed.Value);
            List<Rule> li = new List<Rule>();
            Rule ru = actualRules[random.Next(actualRules.Count)];
            li.Add(ru);
            while (ru.SubRules.Count > 0)
            {
                ru = ru.SubRules[random.Next(ru.SubRules.Count)];
                li.Add(ru);
            }

            if (addToHistory)
            {
                PushHistoryItem(new HistoryItem(HistoryType.Random, seed));
            }

            list.ItemsSource = li;

            LogEvent(EventType.RandomRule);
        }

        private void ShowByKey(string key, bool addToHistory = true)
        {
            int pos = key.IndexOf('.');

            if (pos == -1)
            {
                ShowByNumber(int.Parse(key));
            }
            else
            {
                List<Rule> source = new List<Rule>();
                Rule mustSee = null;
                foreach (Rule ru in actualRules)
                {
                    if (ru.Title.Equals(key.Substring(0, 1) + "."))
                    {
                        source.Add(ru);
                        foreach (Rule ru2 in ru.SubRules)
                        {
                            if (ru2.Title.Equals(key.Substring(0, pos + 1)))
                            {
                                source.Add(ru2);
                                foreach (Rule ru3 in ru2.SubRules)
                                {
                                    source.Add(ru3);
                                    if (mustSee == null && ru3.Title.StartsWith(key))
                                    {
                                        mustSee = ru3;
                                    }
                                }
                                break;
                            }
                        }
                        break;
                    }
                }

                list.ItemsSource = source;
                if (mustSee != null)
                {
                    //list.ScrollIntoView(mustSee, ScrollIntoViewAlignment.Leading);
                    list.SelectedItem = mustSee;
                    list.ScrollTo(mustSee, ScrollToPosition.Center, true);
                    //list.UpdateLayout();
                }

                if (addToHistory)
                {
                    PushHistoryItem(new HistoryItem(HistoryType.Key, key));
                }
            }
        }

        private void ShowByNumber(int number = 0, bool addToHistory = true)
        {
            if (actualRules == null)
                return;
            List<Rule> source = new List<Rule>();
            if (number == 0)
            {
                source = actualRules;
            }
            else if (number >= 1 && number <= 9)
            {
                foreach (var rule in actualRules)
                {
                    if (rule.Title.StartsWith(number + "."))
                    {
                        source.Add(rule);
                        foreach (var rule2 in rule.SubRules)
                        {
                            source.Add(rule2);
                        }
                        break;
                    }
                }
            }
            else if (number >= 100 && number <= 999)
            {
                int high = number / 100;
                foreach (var rule in actualRules)
                {
                    if (rule.Title.StartsWith(high + "."))
                    {
                        source.Add(rule);
                        foreach (var rule2 in rule.SubRules)
                        {
                            if (rule2.Title.StartsWith(number + "."))
                            {
                                source.Add(rule2);
                                source.AddRange(rule2.SubRules);
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            else if (number == 10)
            {
                source.Add(actualRules.Last());
                source.AddRange(actualRules.Last().SubRules);
            }

            if (source.Count > 0)
            {
                if (addToHistory)
                {
                    PushHistoryItem(new HistoryItem(HistoryType.Number, number));
                }
                
                list.ItemsSource = source;
            }
        }

        private async void Search(string text, bool addToHistory = true)
        {
            if (actualRules == null)
                return;
            if (text == null || text.Length < 3)
            {
                await DisplayAlert(MainResources.error, MainResources.searchTerm3Characters, MainResources.ok);
                return;
            }

            using (ActivityIndicatorViewer.Show())
            {

                List<Rule> source = new List<Rule>();

                foreach (var rule in actualRules)
                {
                    if (rule.Title.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        rule.Text.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)
                        source.Add(rule);
                    foreach (var rule2 in rule.SubRules)
                    {
                        if (rule2.Title.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0 ||
                            rule2.Text.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)
                            source.Add(rule2);
                        source.AddRange(
                            rule2.SubRules.Where(
                                rule3 => rule3.Title.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0
                                      || rule3.Text.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0
                            )
                        );
                    }
                }

                if (addToHistory)
                {
                    PushHistoryItem(new HistoryItem(HistoryType.Search, text));
                }

                list.ItemsSource = source;

                LogEvent(EventType.SearchText);
            }
        }

        private void PushHistoryItem(HistoryItem historyItem)
        {
            /*SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                AppViewBackButtonVisibility.Visible;
            ScrollViewer scrollViewer =
                VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(list, 0), 0) as ScrollViewer;
            if (scrollViewer != null)
                actualSearch.VerticalOffset = scrollViewer.VerticalOffset;*/

            if(actualHistoryItem == null || historyItem == null || (!actualHistoryItem.Type.Equals(historyItem.Type) || !actualHistoryItem.Value.Equals(historyItem.Value)))
            {
                if (actualHistoryItem != null)
                {
                    history.Add(actualHistoryItem);

                    backButton.IsEnabled = true;
                }

                actualHistoryItem = historyItem;

                HomeCommand.ChangeCanExecute();
            }
        }

        private bool PopHistoryItem()
        {
            if (history.Count >= 1)
            {
                HistoryItem item = history.Last();
                switch (item.Type)
                {
                    case HistoryType.Search:
                        Search((string)item.Value, false);
                        break;
                    case HistoryType.Number:
                        ShowByNumber((int)item.Value, false);
                        break;
                    case HistoryType.Key:
                        ShowByKey((string)item.Value, false);
                        break;
                    case HistoryType.Random:
                        ShowRandomRule((int?)item.Value, false);
                        break;
                }

                /*list.UpdateLayout();
                ScrollViewer scrollViewer = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(list, 0), 0) as ScrollViewer;
                scrollViewer?.ChangeView(null, item.VerticalOffset, null, true);*/
                //list.ScrollTo(item.Value, ScrollToPosition.Center, true);
                //list.SelectedItem = item.Value;

                history.RemoveAt(history.Count() - 1);
                actualHistoryItem = item;

                backButton.IsEnabled = history.Count > 0;

                HomeCommand.ChangeCanExecute();

                return true;
            }

            return false;
        }

        private void ClearHistory()
        {
            history.Clear();
            actualHistoryItem = null;

            backButton.IsEnabled = false;

            HomeCommand.ChangeCanExecute();
            ClearCacheCommand.ChangeCanExecute();
        }

        private async Task<List<Rule>> GetRulesAsync(RulesSource rulesSource)
        {
            List<Rule> rules = await Rule.getRules(rulesSource);

            if(rules != null)
            {
                existsCache = true;

                ClearCacheCommand.ChangeCanExecute();
            }

            return rules;
        }

        private async Task<bool> LoadDataAsync(int rulesIndex)
        {
            RulesSource source = RulesVersionsService.RulesSources[rulesIndex];
            List<Rule> tempRules = await GetRulesAsync(source);

            if (tempRules == null)
            {
                return false;
            }

            actualRules = tempRules;
            //actualRulesTextBlock.Text = source.Date.ToString("dd/MM/yyyy") + 
            //                            (rulesIndex == RulesVersionsService.RulesSources.Count - 1 ? " (" + MainResources.newest + ")" : "");
            actualRulesIndex = rulesIndex;

            ClearHistory();
            ShowByNumber(0);

            return true;
        }
    }
}
