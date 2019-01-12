using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Core
{
    /// <summary>
    /// Splits a single long message into a series of Tweets 
    /// </summary>
    public class Splitter
    {
        private readonly SplitterConfiguration _splitterConfiguration;
        /// <summary>
        /// Initializes a new instance of the <see cref="Splitter"/> class.
        /// </summary>
        /// <param name="splitterConfiguration">The splitter configuration.</param>
        /// <exception cref="NotImplementedException"></exception>
        public Splitter(SplitterConfiguration splitterConfiguration)
        {
            _splitterConfiguration = splitterConfiguration;   
        }

        /// <summary>
        /// Splits the specified message into a series of tweets.
        /// </summary>
        /// <param name="message">The message to be split.</param>
        /// <returns>A series of tweets to be posted to Twitter, in the order in which they should be posted.</returns>
        /// <exception cref="NotImplementedException"></exception>

        public IEnumerable<string> Split(string message)
        {
            var tweets = new List<string>();
            var sb = new StringBuilder();

            List<string> words = message.Split(' ').ToList();
            List<string> adjustedWords = words;
            var atMetions = new List<string>();

            for(int i = 0; i < adjustedWords.Count;)
            {
                if ((adjustedWords[i].Substring(0, 1).ToString() != "@" && ((atMetions.Count) + 1 != 1)) ||
                    (adjustedWords[i]).Substring(0,1).ToString() == "@" && ((atMetions.Count + 1) > 1))
                {
                    if (adjustedWords[i].Contains("http"))
                    {
                        var url = adjustedWords[i].Substring(0,_splitterConfiguration.ShortenedUrlCharacterLength);

                        sb.Append(url.ToString() + " ");
                    }
                    else
                    {
                        sb.Append(adjustedWords[i] + " ");
                    }
                }

                if (adjustedWords[i].Substring(0,1).ToString() == "@")
                {
                    atMetions.Add(adjustedWords[i]);
                }

                

                if (sb.Length > _splitterConfiguration.MaximumTweetLength)
                {
                    var sbShortened = sb.ToString().Substring(0, sb.Length - (adjustedWords[i].Length + 1));

                    var tweetToAdd = FormatSingleTweet(sbShortened, tweets.Count);

                    tweets.Add(tweetToAdd);

                    sb.Length = 0;
                }
                else if (adjustedWords.Count == 1)
                {
                    var tweetToAdd = FormatSingleTweet(sb.ToString(), tweets.Count);

                    tweets.Add(tweetToAdd);

                    adjustedWords.Remove(adjustedWords[i]);
                }
                else
                {
                    adjustedWords.Remove(adjustedWords[i]);
                }
            }

            var tweetsWithTotal = FormatListOfTweets(tweets, atMetions);

            return tweetsWithTotal;
        }  
        
        private string FormatSingleTweet(string tweet, int currentTweetCount)
        {
            var sb = new StringBuilder(_splitterConfiguration.TweetFormat);

            sb.Replace("{index}", (currentTweetCount + 1).ToString());
            sb.Replace("{message}", tweet);

            return sb.ToString();
        }

        private List<string> FormatListOfTweets(List<string> tweets, List<string> atMentions)
        {
            var tweetsWIthTotal = new List<string>();
            var totalTweets = tweets.Count();

            foreach (var tweet in tweets)
            {
                var sb = new StringBuilder(tweet);

                if (tweet != tweets[0] && tweet != tweets[totalTweets - 1])
                {
                    sb.Replace("{continuation}", "< ");
                    sb.Replace("{continues}", ">");
                }
                else if(tweet == tweets[0])
                {
                    sb.Replace("{continuation}", "");
                    sb.Replace("{continues}", ">");
                }
                else if(tweet == tweets[totalTweets - 1])
                {
                    sb.Replace("{continuation}", "< ");
                    sb.Replace("{continues}", "");
                }

                sb.Replace("{total}", (tweets.Count).ToString());

                if (atMentions != null && atMentions.Any())
                {
                    var sbMentions = new StringBuilder();

                    foreach (var word in atMentions)
                    {
                        sbMentions.Append(word + " ");
                    }

                    sb.Replace("{mention}", sbMentions.ToString());
                }
                else
                {
                    sb.Replace("{mention}", "");
                }

                tweetsWIthTotal.Add(sb.ToString());
            }

            return tweetsWIthTotal;
        }
    }
}
