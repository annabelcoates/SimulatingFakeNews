import pandas as pd
import matplotlib.pyplot as plt
import numpy as np

df=pd.read_csv("C:/Users/Anni/Documents/Uni/Computer Science/Proj/CSVs and "
               "text files/NsharesPopulation.csv")


x = df['nFollowers']
plt.hist(x, bins=50)
plt.gca().set(title='Frequency Histogram', ylabel='Frequency')
plt.show()
nLess200=len(df[(df['nFollowers']<=200)])
n200to500=len(df[(df['nFollowers']<=500) & (df['nFollowers']>200)])
nMore500=len(df[(df['nFollowers']>500)])

nLess200 /= len(df)
n200to500 /= len(df)
nMore500 /= len(df)

objects=('0-200','200-500','500+')
y_pos=np.arange(len(objects))
values=[nLess200,n200to500,nMore500]

plt.bar(y_pos, values, align='center', alpha=0.5)
plt.xticks(y_pos, objects)
plt.show()