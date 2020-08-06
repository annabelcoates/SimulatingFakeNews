import pandas as pd
import networkx as nx
import matplotlib.pyplot as plt

df=pd.read_csv("C:/Users/Anni/Documents/Uni/Computer Science/Proj/CSVs and "
               "text files/followsFacebookUK.csv")
print("1")
G=nx.from_pandas_edgelist(df)

info_df=pd.read_csv("C:/Users/Anni/Documents/Uni/Computer Science/Proj/CSVs and "
               "text files/NsharesPopulation.csv")

colours=info_df["nFakeShares"]
colours2=info_df["nFakeShares"]**2
#colours=colours/max_c #normalize
pos=nx.kamada_kawai_layout(G)
#nx.draw(G,with_labels=False,pos,
#       node_size=10,linewidths=0.1,width=0.01, node_color=colours,
#      cmap=plt.cm.gnuplot)

cmap=plt.cm.cool
vmin = min(colours2)
vmax = max(colours2)
nx.draw(G, pos, node_color=colours2,linewidths=0.1, width=0.01, cmap=cmap,
           with_labels=False, vmin=vmin, vmax=vmax, node_size=10)
sm = plt.cm.ScalarMappable(cmap=cmap, norm=plt.Normalize(vmin = vmin, vmax=vmax))
sm._A = []
plt.colorbar(sm)
plt.show()


plt.show()
