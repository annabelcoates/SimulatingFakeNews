import pandas as pd
import matplotlib.pyplot as plt
import numpy as np


nShared=pd.read_csv("C:/Users/Anni/Documents/Uni/Computer Science/Proj/CSVs "
                    "and text files/nSharedFakeNews.csv")
timeStamps=range(0,nShared.size*15,15)
fake_news=[0]*100 # create an empty array to fill with dataframes
true_news=[0]*100# create an empty array to fill with dataframes

for i in range(10):
    fake_news[i]=np.array(pd.read_csv("C:/Users/Anni/Documents/Uni/Computer "
                        "Science/Proj/CSVs "
                    "and text files/nShared"+str(i)+".csv"))

for i in range (10,20,1):
    true_news[i-10]= np.array(pd.read_csv(
        "C:/Users/Anni/Documents/Uni/Computer "
                        "Science/Proj/CSVs "
                    "and text files/nShared"+str(i)+".csv"))

fake_array=np.array([fake_news[0],fake_news[1],fake_news[2],
                     fake_news[3],
                     fake_news[4],fake_news[5],fake_news[6],fake_news[7],
                     fake_news[8],fake_news[9]])
fake_average=np.mean(fake_array,axis=0)
true_array=np.array([true_news[0],true_news[1],true_news[2],
                              true_news[3],
                              true_news[4],true_news[5],true_news[6],true_news[7],
                              true_news[8],true_news[9]])
true_average=np.mean(true_array,axis=0)
plt.plot(fake_average, label=" fake (e=1, b=0.1)")
plt.plot(true_average,label="true (e=0.5, b=1)")
plt.legend()
plt.xlabel("Simulation minutes")
plt.ylabel("Number of accounts that shared")
plt.title("Spread of news for simple personality features (small world "
          "structure)")
plt.show()