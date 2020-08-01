import pandas as pd
import networkx as nx
import matplotlib.pyplot as plt

df= pd.read_csv("C:/Users/Anni/Documents/Uni/Computer " \
                 "Science/Proj/facebook_combined.txt/facebook_combined.csv")
G = nx.from_pandas_edgelist(df)


G2 = nx.watts_strogatz_graph(10000,500,0.9)
print("here")
nx.draw(G2,with_labels=False,pos=nx.spectral_layout(G2),
       node_size=20,linewidths=0.1,width=0.01,node_color=range(10000),
      cmap=plt.cm.Blues)
plt.show()