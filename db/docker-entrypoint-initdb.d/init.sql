CREATE TABLE IF NOT EXISTS Account (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    password VARCHAR(255),
    display_name VARCHAR(100),
    email VARCHAR(100)
);


CREATE INDEX IF NOT EXISTS idx_account_username ON Account(username);

ALTER TABLE Account 
ADD CONSTRAINT email_check CHECK (email ~* '^[A-Za-z0-9._%-]+@[A-Za-z0-9.-]+[.][A-Za-z]+$');

-- password: "12345678"
INSERT INTO Account (username, password, display_name, email)
VALUES 
    ('admin', '$2a$11$goqU7elF8WKtZs1/8fBDB.zrMlNeqYLW2PG0fAciWTgmnYALTlTey', 'Administrator', 'admin@example.com'),
    ('user1', '$2a$11$goqU7elF8WKtZs1/8fBDB.zrMlNeqYLW2PG0fAciWTgmnYALTlTey', 'Regular User', 'user1@example.com')
ON CONFLICT (username) DO NOTHING;