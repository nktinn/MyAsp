using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Net.Http;
using System.Globalization;

using MyAsp.Storage;
using MyAsp.Logic.Accounts;

using Microsoft.EntityFrameworkCore;

namespace MyAsp.Logic.HostedService
{
    public class HostedManager : IHostedManager
    {
        private readonly Context _context;
        public HostedManager(Context context)
        {
            _context = context;
        }
        public async Task RemoveExpiredTempPasswords()
        {
            var accounts = _context.Users.ToList();
            var dateNow = DateTime.Now;

            foreach (var account in accounts)
            {
                if (account.TmpPassword != null && account.RequestTime.HasValue)
                {
                    DateTime timeOfRequest = Convert.ToDateTime(account.RequestTime);
                    timeOfRequest.AddHours(2);
                    int result = DateTime.Compare(dateNow, timeOfRequest);
                    if (result >= 0)
                    {
                        account.TmpPassword = null;
                        account.RequestTime = null;
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task ParsePages()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                string url = "https://stankin.ru/pages/id_79/news_1";

                //Загружаем страницу
                driver.Navigate().GoToUrl(url);

                // Выбираем все элементы с классом "news-list-item"
                var newsListItems = driver.FindElements(By.XPath("//div[contains(@class, \"news-list-item\")]"))
                    .Take(6); // Получаем только первые 6 элементов

                if (newsListItems.Count() != 0)
                {
                    await ClearNews();

                    foreach (var listItem in newsListItems)
                    {
                        New newsItem = new New();

                        // Извлекаем информацию из внутренних элементов и заносим в объект NewsItem
                        newsItem.Info = listItem.FindElement(By.TagName("img")).GetAttribute("alt");
                        newsItem.Link = listItem.FindElement(By.TagName("a")).GetAttribute("href");
                        var titleElement = listItem.FindElement(By.TagName("p")).Text;
                        newsItem.Date = DateOnly.ParseExact(titleElement, "d.M.yyyy", CultureInfo.InvariantCulture);


                        _context.News.Add(newsItem);
                    }
                }
                await _context.SaveChangesAsync();
                driver.Quit();
            }
        }
        public async Task DeleteExams()
        {
            var exams = await _context.Results.Where(e => e.Type != "2").ToListAsync();
            var timetables = await _context.Timetables.ToListAsync();

            _context.Results.RemoveRange(exams);
            _context.Timetables.RemoveRange(timetables);

            await _context.SaveChangesAsync();
        }
        public async Task DeleteExpiredGrants()
        {

            var grants = await _context.Grants.ToListAsync();

            foreach (var grant in grants)
            {
                var dateNow = DateOnly.FromDateTime(DateTime.Now);
                if (dateNow > grant.ExpiryDate)
                {
                    _context.Grants.Remove(grant);
                }
            }

            await _context.SaveChangesAsync();
        }


        public async Task ClearNews()
        {
            var allEntities = await _context.News.ToListAsync();
            _context.News.RemoveRange(allEntities);
            await _context.SaveChangesAsync();
        }
    }
}
