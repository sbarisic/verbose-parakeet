create table ParakeetUser (
	ID INT IDENTITY PRIMARY KEY ,
	Username VARCHAR(256) NOT NULL UNIQUE,
	Salt VARCHAR(512),
	Hash VARCHAR(512)
);

create table ContentID (
	ID INT IDENTITY PRIMARY KEY,
	OwnerID INT,							-- Content owner ID, linked to table ParakeetUser

	Name VARCHAR(256),						-- Content name
	Description VARCHAR(8000),				-- Content description
	ContentType INT NOT NULL DEFAULT 0,
	FileName VARCHAR(512)
);

alter table ContentID add Tags VARCHAR(512) null; 


/


select * from ParakeetUser;