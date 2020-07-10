using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Data.Analysis;
namespace ModelAttemptWPF
{
    public class OSN
    {
        private readonly MainWindow window; //Maybe this should be public and just the osn as a whole can be passed to the new Account() method

        public bool[,] followArray =new bool[,] { };
        public DataFrame followDF = new DataFrame();
        public List<Account> accountList = new List<Account>();
        public int IDCount = 0;
        public OSN(MainWindow window)
        {
            this.window = window;
        }

        public Account NewAccount(string name,int freqUse)
        {
            Account newAccount = new Account(this.window, this, name, freqUse);
            IDCount++;
            accountList.Add(newAccount);
            return newAccount;
        }

        public void Follow(Account follower, Account beingFollowedAccount)
        {
            this.followArray[follower.ID, beingFollowedAccount.ID] = true;
            follower.following.Add(beingFollowedAccount);
            beingFollowedAccount.followers.Add(follower);
        }

        
    }
}

