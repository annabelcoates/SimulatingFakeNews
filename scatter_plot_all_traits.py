import numpy as np
import pandas as pd
np.random.seed(19680801)
import matplotlib.pyplot as plt

df=pd.read_csv("C:/Users/Anni/Documents/Uni/Computer Science/Proj/CSVs and "
               "text files/NsharesPopulation.csv")
for trait in ['o','c','e','a','n','Online Literacy','nFollowers','Political '
                                                                 'Leaning']:
    fig, ax = plt.subplots()
    for index, row in df.iterrows():
        x, y = [row[trait],row['nFakeShares']]
        ax.scatter(x, y,alpha=0.2, edgecolors='none',color='r')

    plt.xlabel(trait)
    plt.ylabel("n fake shares")
    #ax.legend()
    ax.grid(True)

plt.show()