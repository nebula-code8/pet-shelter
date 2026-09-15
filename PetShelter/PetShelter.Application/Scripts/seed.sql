-- =========================================================
-- USERS
-- role:
-- 0 = CLIENT
-- 1 = VOLUNTEER
-- 2 = ADMIN
-- 3 = ASSOCIATION_ADMIN
-- gender:
-- 0 = MALE
-- 1 = FEMALE
-- =========================================================

INSERT INTO users
(name, surname, gender, date_of_birth, phone_number, email, password, role, address, is_deleted)
VALUES
    ('Marko', 'Petrović', 0, '1998-05-12', '+381641112233',
     'marko.petrovic@gmail.com', 'password123', 0,
     'Bulevar Nemanjića 12, Niš', FALSE),

    ('Ana', 'Jovanović', 1, '2000-08-21', '+381642223344',
     'ana.jovanovic@gmail.com', 'password123', 0,
     'Vizantijski bulevar 45, Niš', FALSE),

    ('Nikola', 'Stojanović', 0, '1995-03-17', '+381633334455',
     'nikola.stojanovic@gmail.com', 'password123', 0,
     'Voždova 23, Niš', FALSE),

    ('Milica', 'Đorđević', 1, '1999-11-02', '+381644445566',
     'milica.djordjevic@gmail.com', 'password123', 0,
     'Bulevar Zorana Đinđića 17, Niš', FALSE),

    ('Luka', 'Milošević', 0, '2001-01-28', '+381655556677',
     'luka.milosevic@gmail.com', 'password123', 0,
     'Dušanova 8, Niš', FALSE),

    ('Jelena', 'Nikolić', 1, '1997-06-14', '+381666667788',
     'jelena.nikolic@gmail.com', 'password123', 1,
     'Somborski bulevar 31, Niš', FALSE),

    ('Stefan', 'Ilić', 0, '1994-09-30', '+381677778899',
     'stefan.ilic@gmail.com', 'password123', 1,
     'Duvanište 14, Niš', FALSE),

    ('Marija', 'Pavlović', 1, '1992-12-05', '+381688889900',
     'marija.pavlovic@gmail.com', 'password123', 2,
     'Medijana 7, Niš', FALSE),

    ('Milan', 'Kostić', 0, '1988-04-17', '+381601111111',
     'milan.kostic@nisanimalrescue.rs', 'password123', 3,
     'Bulevar Svetog cara Konstantina 42, Niš', FALSE),

    ('Ivana', 'Savić', 1, '1991-07-23', '+381602222222',
     'ivana.savic@happypaws.rs', 'password123', 3,
     'Dimitrija Tucovića 18, Niš', FALSE),

    ('Nemanja', 'Marković', 0, '1986-10-09', '+381603333333',
     'nemanja.markovic@safetails.rs', 'password123', 3,
     'Bulevar Nikole Tesle 30, Niš', FALSE);

-- =========================================================
-- ASSOCIATIONS
-- =========================================================

INSERT INTO associations
(name, date_of_establishment, phone_number, email, tip, description, address, admin, is_deleted)
VALUES
    (
        'Niš Animal Rescue',
        '2015-04-12',
        '+381184001001',
        'contact@nisanimalrescue.rs',
        'Animal Rescue',
        'Association dedicated to rescuing, treating and finding homes for abandoned animals.',
        'Bulevar Svetog cara Konstantina 80, Niš',
        9,
        FALSE
    ),

    (
        'Happy Paws',
        '2018-09-20',
        '+381184002002',
        'info@happypaws.rs',
        'Animal Shelter',
        'Local animal shelter providing temporary homes and veterinary care for abandoned pets.',
        'Dimitrija Tucovića 25, Niš',
        10,
        FALSE
    ),

    (
        'Safe Tails',
        '2020-02-15',
        '+381184003003',
        'office@safetails.rs',
        'Rescue Organization',
        'Non-profit organization focused on rescuing dogs and cats from dangerous environments.',
        'Bulevar Nikole Tesle 42, Niš',
        11,
        FALSE
    );

-- =========================================================
-- BANK ACCOUNTS
-- =========================================================

INSERT INTO bank_accounts
(association_id, account_number, balance)
VALUES
    (
        1,
        '160-1000000000001-01',
        50000.00
    ),
    (
        2,
        '160-1000000000002-02',
        75000.00
    ),
    (
        3,
        '160-1000000000003-03',
        40000.00
    );

-- =========================================================
-- BANK TRANSACTIONS
--
-- transaction_type:
-- 0 = DONATION
-- 1 = EXPENSE
-- =========================================================

INSERT INTO bank_transactions
(
    bank_account_id,
    transaction_type,
    amount,
    transaction_date,
    source_account,
    destination_account,
    purpose,
    description
)
VALUES

-- Donation to Niš Animal Rescue
(
    1,
    0,
    20000.00,
    '2026-09-01',
    '160-3000000000001-01',
    '160-1000000000001-01',
    'Donation',
    'Donation for animal care'
),

-- Expense by Niš Animal Rescue
(
    1,
    1,
    5000.00,
    '2026-09-03',
    '160-1000000000001-01',
    '160-2000000000001-01',
    'Veterinary treatment',
    'Treatment for rescued dog'
),

-- Donation to Happy Paws
(
    2,
    0,
    30000.00,
    '2026-09-02',
    '160-3000000000002-02',
    '160-1000000000002-02',
    'Donation',
    'Donation for animal food'
),

-- Expense by Happy Paws
(
    2,
    1,
    8000.00,
    '2026-09-05',
    '160-1000000000002-02',
    '160-2000000000002-02',
    'Animal food',
    'Purchase of dog and cat food'
),

-- Donation to Safe Tails
(
    3,
    0,
    15000.00,
    '2026-09-04',
    '160-3000000000003-03',
    '160-1000000000003-03',
    'Donation',
    'Donation for rescued animals'
);

-- =========================================================
-- VOLUNTEERS
--
-- status:
-- 0 = PENDING
-- 1 = APPROVED
-- 2 = REJECTED
-- =========================================================

INSERT INTO volunteers
(user_id, association_id, comment, status)
VALUES
    (
        6,
        1,
        'I have experience working with dogs and cats.',
        1
    ),
    (
        7,
        2,
        'I would like to help with animal care and walking dogs.',
        1
    );

-- =========================================================
-- ANIMALS
--
-- gender:
-- 0 = MALE
-- 1 = FEMALE
--
-- is_vacdinated:
-- 0 = NO
-- 1 = YES
--
-- is_sterilized:
-- 0 = NO
-- 1 = YES
-- =========================================================

INSERT INTO animals
(
    species,
    breed,
    gender,
    date_of_birth,
    description,
    is_vacdinated,
    is_sterilized,
    health_status,
    date_arrived,
    association_id,
    is_deleted
)
VALUES

-- DOGS
(
    'Dog',
    'Labrador Retriever',
    0,
    '2021-05-14',
    'Friendly and energetic dog who loves playing with people.',
    1,
    1,
    'Healthy',
    '2025-02-10',
    1,
    FALSE
),

(
    'Dog',
    'German Shepherd',
    1,
    '2020-09-22',
    'Calm and intelligent female dog. Good with experienced owners.',
    1,
    1,
    'Healthy',
    '2025-03-18',
    1,
    FALSE
),

(
    'Dog',
    'Mixed Breed',
    0,
    '2022-07-03',
    'Very playful young dog that enjoys long walks.',
    1,
    0,
    'Healthy',
    '2025-05-12',
    2,
    FALSE
),

(
    'Dog',
    'Beagle',
    1,
    '2019-04-16',
    'Friendly and curious dog with a lot of energy.',
    1,
    1,
    'Healthy',
    '2025-06-01',
    2,
    FALSE
),

(
    'Dog',
    'Husky',
    0,
    '2020-12-10',
    'Active dog that needs an owner who enjoys outdoor activities.',
    1,
    0,
    'Healthy',
    '2025-07-21',
    3,
    FALSE
),

(
    'Dog',
    'Mixed Breed',
    1,
    '2023-02-28',
    'Small and affectionate female dog. Good with children.',
    1,
    1,
    'Healthy',
    '2025-08-03',
    3,
    FALSE
),

-- CATS
(
    'Cat',
    'Domestic Shorthair',
    1,
    '2022-01-11',
    'Quiet and affectionate cat who enjoys being around people.',
    1,
    1,
    'Healthy',
    '2025-01-20',
    1,
    FALSE
),

(
    'Cat',
    'British Shorthair',
    0,
    '2021-10-05',
    'Calm indoor cat with a friendly personality.',
    1,
    1,
    'Healthy',
    '2025-04-08',
    1,
    FALSE
),

(
    'Cat',
    'Maine Coon',
    0,
    '2020-06-18',
    'Large and social cat. Loves attention and playing.',
    1,
    0,
    'Healthy',
    '2025-05-25',
    2,
    FALSE
),

(
    'Cat',
    'Domestic Longhair',
    1,
    '2023-03-12',
    'Young and playful cat that likes climbing and toys.',
    1,
    1,
    'Healthy',
    '2025-07-14',
    2,
    FALSE
),

(
    'Cat',
    'Siamese',
    1,
    '2019-11-09',
    'Very social and vocal cat. Needs an attentive owner.',
    1,
    1,
    'Healthy',
    '2025-08-19',
    3,
    FALSE
),

-- ONE ANIMAL WITH A MINOR HEALTH ISSUE
(
    'Dog',
    'Mixed Breed',
    0,
    '2018-05-30',
    'Older dog with a calm personality and gentle temperament.',
    1,
    1,
    'Mild arthritis, currently receiving treatment',
    '2025-09-02',
    3,
    FALSE
);


-- =========================================================
-- ADOPTION REQUESTS
--
-- status:
-- 0 = PENDING
-- 1 = APPROVED
-- 2 = REJECTED
-- =========================================================

INSERT INTO adoption_requests
(user_id, animal_id, status, adoption_date, request_date)
VALUES

-- Marko wants to adopt the Labrador
(1, 1, 1, '2025-09-10', '2025-09-01'),

-- Ana wants to adopt the German Shepherd
(2, 2, 0, NULL, '2025-09-05'),

-- Nikola wants to adopt the mixed breed dog
(3, 3, 0, NULL, '2025-09-07'),

-- Milica wants to adopt the Beagle
(4, 4, 2, NULL, '2025-08-20'),

-- Luka wants to adopt the Husky
(5, 5, 0, NULL, '2025-09-08'),

-- Ana also wants to adopt a cat
(2, 7, 1, '2025-09-12', '2025-09-03'),

-- Nikola wants to adopt the British Shorthair
(3, 8, 0, NULL, '2025-09-09'),

-- Milica wants to adopt the Maine Coon
(4, 9, 0, NULL, '2025-09-10'),

-- Luka wants to adopt the Siamese
(5, 11, 2, NULL, '2025-08-25'),

-- Marko wants to adopt another dog
(1, 6, 0, NULL, '2025-09-11');