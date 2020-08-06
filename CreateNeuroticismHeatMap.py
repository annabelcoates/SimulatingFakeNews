import pandas as pd
import networkx as nx
import matplotlib.pyplot as plt


df=pd.read_csv("C:/Users/Anni/Documents/Uni/Computer Science/Proj/CSVs and "
               "text files/followsFacebookUK.csv")
G=nx.from_pandas_edgelist(df)
pos = nx.kamada_kawai_layout(G)
count=1
def create_plot(trait, title, n):
    cmap = plt.cm.cool
    colours=info_df[trait]
    size=(info_df["nFollowers"]**2)/250
    vmin = min(colours)
    vmax = max(colours)
    plt.figure(n)
    nx.draw(G, pos, node_color=colours, linewidths=0.1, width=0.01, cmap=cmap,
            with_labels=False, vmin=vmin, vmax=vmax, node_size=size)
    sm = plt.cm.ScalarMappable(cmap=cmap,
                               norm=plt.Normalize(vmin=vmin, vmax=vmax))
    sm._A = []
    plt.colorbar(sm)
    plt.title(title)



info_df=pd.read_csv("C:/Users/Anni/Documents/Uni/Computer Science/Proj/CSVs and "
               "text files/NsharesPopulation.csv")


print("about to create plot")
create_plot("o", "Openness",1)
create_plot("e","Extroversion",2)
create_plot("n","Neuroticism",3)
create_plot("c","Conscientiousness",4)
plt.show()


