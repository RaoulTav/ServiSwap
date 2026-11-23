USE serviswap;

INSERT INTO Addresses (streetNumber, streetName, city)
VALUES
    ('12', 'Rue de la Station', 'Charleroi'),
    ('45', 'Avenue Louise', 'Bruxelles'),
    ('8', 'Rue des Brasseurs', 'Namur'),
    ('27', 'Rue de la Cathédrale', 'Liège'),
    ('103', 'Chaussée de Mons', 'Mons'),
    ('16', 'Rue du Commerce', 'Tournai'),
    ('72', 'Avenue de la Gare', 'La Louvière'),
    ('5', 'Rue des Jardins', 'Nivelles'),
    ('31', 'Rue de l''Église', 'Wavre'),
    ('89', 'Rue du Moulin', 'Dinant');

INSERT INTO Categories (categoryName)
VALUES
	('domestiques'),
    ('créatifs'),
    ('bien-être'),
    ('de bricolage');

INSERT INTO People (username, password, lastName, firstName, email, creditHours, latitude, longitude, addressId)
VALUES
    ('NoxCharleroi', 'P@ssword123', 'Dupont', 'Thomas', 'thomas.dupont@example.com', 10, 50.4120, 4.4440, 1),
    ('AliceBruxelles', 'P@ssword123', 'Martin', 'Alice', 'alice.martin@example.com', 10, 50.8260, 4.3659, 2),
    ('MarcNamur01', 'P@ssword123', 'Lambert', 'Marc', 'marc.lambert@example.com', 10, 50.4622, 4.8675, 3),
    ('JulieLiege01', 'P@ssword123', 'Leclercq', 'Julie', 'julie.leclercq@example.com', 10, 50.6426, 5.574864, 4),
    ('PaulMons01', 'P@ssword123', 'Simon', 'Paul', 'paul.simon@example.com', 10, 50.4540, 3.9520, 5),
    ('EmmaTournai', 'P@ssword123', 'Moreau', 'Emma', 'emma.moreau@example.com', 10, 50.6050, 3.3880, 6),
    ('LucasLouviere', 'P@ssword123', 'Robert', 'Lucas', 'lucas.robert@example.com', 10, 50.4780, 4.1870, 7),
    ('SarahNivelles', 'P@ssword123', 'Bernard', 'Sarah', 'sarah.bernard@example.com', 10, 50.5980, 4.3280, 8),
    ('HugoWavre01', 'P@ssword123', 'Gérard', 'Hugo', 'hugo.gerard@example.com', 10, 50.7160, 4.6100, 9),
    ('LauraDinant', 'P@ssword123', 'Michel', 'Laura', 'laura.michel@example.com', 10, 50.2600, 4.9120, 10);

INSERT INTO Services (serviceName, serviceDescription, dateOfPublication, categoryId, providerId)
VALUES
    -- Person 1
    ('Aide au jardinage', 'Aide pour tondre la pelouse, désherber, planter des fleurs ou entretenir un petit jardin.', '2026-08-01', 1, 1), 
    ('Montage de meubles', 'Aide au montage de meubles en kit, étagères, bureaux ou petits meubles domestiques.', '2026-08-03', 4, 1), 
    -- Person 2
    ('Aide au ménage', 'Aide pour effectuer différentes tâches ménagères comme le nettoyage, le rangement ou le dépoussiérage.', '2026-08-02', 1, 2), 
    ('Initiation au dessin', 'Initiation aux bases du dessin et accompagnement pour réaliser des croquis simples.', '2026-08-05', 2, 2), 
    -- Person 3
    ('Promenade de chiens', 'Promenade de chiens pendant une heure, dans le quartier ou dans un parc proche.', '2026-08-04', 1, 3), 
    ('Création de cartes personnalisées', 'Création de cartes personnalisées pour anniversaires, fêtes ou autres occasions.', '2026-08-06', 2, 3), 
    -- Person 4
    ('Aide au rangement', 'Aide à trier, organiser et ranger différentes pièces de la maison.', '2026-08-05', 1, 4), 
    ('Initiation à la méditation', 'Séance d''initiation à la méditation et présentation de différentes techniques de relaxation.', '2026-08-07', 3, 4), 
    -- Person 5
    ('Petites réparations', 'Aide pour effectuer de petites réparations domestiques et résoudre des problèmes courants.', '2026-08-06', 4, 5), 
    ('Montage d''étagères', 'Aide à l''installation et au montage d''étagères murales et de petits éléments de rangement.', '2026-08-08', 4, 5), 
    -- Person 6
    ('Préparation de repas', 'Aide à la préparation de repas simples et organisation des ingrédients pour plusieurs jours.', '2026-08-07', 1, 6), 
    ('Aide à la peinture', 'Aide pour préparer et peindre les murs d''une pièce ou réaliser de petites retouches.', '2026-08-09', 4, 6), 
    -- Person 7
    ('Création de décorations', 'Création de petites décorations personnalisées pour la maison ou pour des événements.', '2026-08-08', 2, 7), 
    ('Atelier origami', 'Découverte de différentes techniques d''origami et réalisation de modèles simples.', '2026-08-10', 2, 7), 
    -- Person 8
    ('Conseils relaxation', 'Échange autour de différentes méthodes permettant de se détendre et de mieux gérer son temps de repos.', '2026-08-09', 3, 8), 
    ('Séance d''étirements', 'Séance douce d''étirements adaptée aux personnes souhaitant améliorer leur souplesse.', '2026-08-11', 3, 8), 
    -- Person 9
    ('Aide au déménagement', 'Aide au déplacement et au transport de cartons et de petits meubles lors d''un déménagement.', '2026-08-10', 1, 9), 
    ('Réparation de petits objets', 'Aide pour réparer ou remettre en état de petits objets et équipements domestiques.', '2026-08-12', 4, 9), 
    -- Person 10
    ('Initiation à la photographie', 'Découverte des bases de la photographie et conseils pour améliorer ses prises de vue.', '2026-08-11', 2, 10), 
    ('Atelier création manuelle', 'Accompagnement dans la réalisation de petits projets créatifs et travaux manuels.', '2026-08-13', 2, 10);
    
INSERT INTO RequestedServices (dateOfRequest, state, reportedHours, requesterId, serviceId)
VALUES
    -- Person 1 demande des services de Person 2 et Person 3
    ('2026-08-14', 1, 0, 1, 3), ('2026-08-15', 1, 0, 1, 5),
    -- Person 2 demande des services de Person 3 et Person 4
    ('2026-08-14', 1, 0, 2, 5), ('2026-08-16', 1, 0, 2, 7),
    -- Person 3 demande des services de Person 4 et Person 5
    ('2026-08-15', 1, 0, 3, 7), ('2026-08-17', 1, 0, 3, 9),
    -- Person 4 demande des services de Person 5 et Person 6
    ('2026-08-16', 1, 0, 4, 9), ('2026-08-18', 1, 0, 4, 11),
    -- Person 5 demande des services de Person 6 et Person 7
    ('2026-08-17', 1, 0, 5, 11), ('2026-08-18', 1, 0, 5, 13),
    -- Person 6 demande des services de Person 7 et Person 8
    ('2026-08-18', 1, 0, 6, 13), ('2026-08-19', 1, 0, 6, 15),
    -- Person 7 demande des services de Person 8 et Person 9
    ('2026-08-18', 1, 0, 7, 15), ('2026-08-19', 1, 0, 7, 17),
    -- Person 8 demande des services de Person 9 et Person 10
    ('2026-08-19', 1, 0, 8, 17), ('2026-08-20', 1, 0, 8, 19),
    -- Person 9 demande des services de Person 10 et Person 1
    ('2026-08-19', 1, 0, 9, 19), ('2026-08-20', 1, 0, 9, 1),
    -- Person 10 demande des services de Person 1 et Person 2
    ('2026-08-20', 1, 0, 10, 1), ('2026-08-21', 1, 0, 10, 3);