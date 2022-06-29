# restaurant

GET
Obtenir toute les commandes : https://localhost:7197/api/restaurants/getAll
Obtenir une commande à l'aide de son id : https://localhost:7197/api/restaurants/getCommande/{ID}
Termine la préparation d'un plat à l'aide de son id : https://localhost:7197/api/restaurants/platok/{ID}
Termine la préparation d'un boisson à l'aide de son id : https://localhost:7197/api/restaurants/boissonok/{ID}
Délivre une commande : https://localhost:7197/api/restaurants/delivred/{ID}

POST
Ajoute une commande : Exemple de json postman : 
"{"name":"walk dog","isComplete":true, "plats":[{"name":"salade"}, {"name":"riz"}], "boissons":[{"name":"jus d'orange"}]}" 

DELETE
Supprimer une commande :  https://localhost:7197/api/restaurants/{ID}
