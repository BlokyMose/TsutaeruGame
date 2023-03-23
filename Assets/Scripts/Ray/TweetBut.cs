using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsutaeru
{
    public class TweetBut : MonoBehaviour
    {
        [SerializeField]
        string tweetText;

        [SerializeField]
        List<string> hashtags = new();

        public void Tweet()
        {
            naichilab.UnityRoomTweet.Tweet("i_love_you_too",tweetText,hashtags.ToArray());
        }
    }
}
