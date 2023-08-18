using MyAsp.Storage;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Net.Http;
using System.Globalization;
using System.Linq;

namespace MyAsp.Logic.Accounts
{
    public class AccountManager: IAccountManager
    {

        private readonly Context _context;
        public AccountManager(Context context)
        {
            _context = context;
        }

        public async Task UpdateContext()
        {
            await _context.SaveChangesAsync();
        }

        public async Task ClearNews()
        {
            var allEntities = _context.News.ToList();
            _context.News.RemoveRange(allEntities);
            await _context.SaveChangesAsync();
        }

        public async Task ParseStankinRU()
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
    }
}
