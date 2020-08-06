import pandas as pd
import matplotlib.pyplot as plt
import numpy as np

nFake=100
nTrue=100
nShared=pd.read_csv("C:/Users/Anni/Documents/Uni/Computer Science/Proj/CSVs "
                    "and text files/nSharedFakeNews.csv")
timeStamps=range(0,nShared.size*15,15)
fake_news_shared= [0] * nFake # create an empty array to fill with dataframes
fake_news_viewed=[0] * nFake
true_news_shared= [0] * nTrue# create an empty array to fill with dataframes
true_news_viewed=[0]*nTrue

for i in range(nFake):
    fake_news_shared[i]=np.array(pd.read_csv("C:/Users/Anni/Documents/Uni/Computer "
                        "Science/Proj/CSVs "
                    "and text files/nShared" + str(i) +".csv"))
    fake_news_viewed[i]=np.array(pd.read_csv("C:/Users/Anni/Documents/Uni/Computer "
                        "Science/Proj/CSVs "
                    "and text files/nViewed" + str(i) +".csv"))

for i in range (nFake,nFake+nTrue,1):
    true_news_shared[i - nFake]= np.array(pd.read_csv(
        "C:/Users/Anni/Documents/Uni/Computer "
                        "Science/Proj/CSVs "
                    "and text files/nShared"+str(i)+".csv"))
    true_news_viewed[i-nFake]=np.array(pd.read_csv(
        "C:/Users/Anni/Documents/Uni/Computer "
                        "Science/Proj/CSVs "
                    "and text files/nViewed" + str(i) +".csv"))


fake_average_s=np.mean(fake_news_shared, axis=0)
fake_average_v=np.mean(fake_news_viewed,axis=0)

true_average_s=np.mean(true_news_shared, axis=0)
true_average_v=np.mean(true_news_viewed,axis=0)
plt.plot(fake_average_s, label="average fake shared")
plt.plot(true_average_s, label="average true shared")
plt.plot(fake_average_v,label="average fake viewed")
plt.plot(true_average_v,label="average true viewed")
plt.legend()
plt.xlabel("Time")
plt.ylabel("Number of accounts that shared")
plt.show()