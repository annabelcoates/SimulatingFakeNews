import numpy as np
import pandas as pd
np.random.seed(19680801)
import matplotlib.pyplot as plt

df=pd.read_csv("C:/Users/Anni/Documents/Uni/Computer Science/Proj/CSVs and "
               "text files/NsharesPopulation.csv")
print(df['nFakeShares'])
fig, ax = plt.subplots()
for index, row in df.iterrows():
    x, y = [row['Online Literacy'],(row['nFakeShares']/row['nTrueShares'])]
    ax.scatter(x, y,alpha=0.3, edgecolors='none',color='b')
    

x1=0.4
y1=np.mean(df[df['Online Literacy']==0.4]["nFakeShares"])/np.mean(df[df[
                                                                         'Online Literacy']==0.4]["nTrueShares"])
ax.scatter(x1,y1,alpha=1,color='w')

x2=0.8
y2=np.mean(df[df['Online Literacy']==0.8]["nFakeShares"])/np.mean(df[df[
                                                                        'Online Literacy']==0.8]["nTrueShares"])
ax.scatter(x2,y2,alpha=1,color='w')

plt.xlabel("OL")
plt.ylabel("ratio fake to true shares")
plt.title("population=1000, 50 fake news and 100 true news")
ax.legend()
ax.grid(True)

plt.show()