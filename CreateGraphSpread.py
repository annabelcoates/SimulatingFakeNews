import pandas as pd
import networkx as nx
import matplotlib.pyplot as plt
import numpy as np

df=pd.read_csv("C:/Users/Anni/Documents/Uni/Computer Science/Proj/CSVs and "
               "text files/followsFacebookUK.csv")
G=nx.from_pandas_edgelist(df)
print("made the graph")
pos = nx.fruchterman_reingold_layout(G)
print("made the pos array")
info_df=pd.read_csv("C:/Users/Anni/Documents/Uni/Computer Science/Proj/CSVs and "
               "text files/NsharesPopulation.csv")
def create_spread_graph(n):
    sharers=np.array(pd.read_csv("C:/Users/Anni/Documents/Uni/Computer "
                         "Science/Proj/CSVs and "
               "text files/sharers"+str(n)+".csv"))
    color_map = []
    for node in G:
        if node in sharers[0] :
            color_map.append('red')
        elif node in sharers:
            color_map.append('orange')
        else:
            color_map.append('lightgrey')
    size = (info_df["nFollowers"] ** 2) / 250
    #vmin = min(colours)
    #vmax = max(colours)
    plt.figure(n)
    nx.draw(G, pos, linewidths=0.1, width=0.01, node_color=color_map,
            with_labels=False,node_size=size,edge_color='black')
    plt.title("Spread of news ID: "+str(n))
print("about to start plotting the graphs")
create_spread_graph(5)
create_spread_graph(200)
plt.show()