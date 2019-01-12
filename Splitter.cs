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
        //public IEnumerable<ITweet> Split(string message)
        //{
        //    throw new NotImplementedException();
        //}

        public IEnumerable<string> Split(string message)
        {
            var tweets = new List<string>();
            var sb = new StringBuilder();

            List<string> words = message.Split(' ').ToList();
            List<string> adjustedWords = words;
           
            for(int i = 0; i < adjustedWords.Count;)
            {
                sb.Append(adjustedWords[i] + " ");

                if (sb.Length > _splitterConfiguration.MaximumTweetLength)
                {
                    var sbShortened = sb.ToString().Substring(0, sb.Length - (adjustedWords[i].Length + 1));

                    tweets.Add(sbShortened);

                    sb.Length = 0;
                }
                else if (adjustedWords.Count == 1)
                {
                    tweets.Add(sb.ToString());

                    adjustedWords.Remove(adjustedWords[i]);
                }
                else
                {
                    adjustedWords.Remove(adjustedWords[i]);

                    i = 0;
                }
            }

            return tweets;
        }  
        
        
    }
}
