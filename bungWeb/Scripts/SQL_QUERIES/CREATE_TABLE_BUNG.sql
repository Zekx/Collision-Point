use bung_site;

create table users(
	id	int auto_increment primary key,
    firstname	varchar(255) not null,
    lastname	varchar(255) not null,
    username	varchar(255) not null unique,
    pass		varchar(255) not null,
    phone		varchar(255) default '',
    email		varchar(255) not null unique,
    position 	int not null
);

create table posts(
    id          int auto_increment primary key,
    userid		int not null,
    CONSTRAINT fk_author foreign key (userid) references users(id), 
    title		text not null,
    body		longtext not null,
    datePosted	date,
    removed		boolean
);

insert into posts (userid, title, body, datePosted, removed) values
    (1, 'Introduction to Collision Point', 
    '<p>Hello there! This website is still under construction but I hope to develop it to be something special.
    The overall design is still a work in progres but hopefully I can get things rolling for my own website.
    Further information about me can be viewed on my resume or my social media accounts on the navigation bar above.
    As for future blog posts, I plan to just post topics about my own programming works, food travels and doodles I
    occassionaly do on my free time.</p>
    
    <p>I consider myself to be sporadic. I like to do many things and try my best on any of those activities.
    Well, I guess I try to find myself to be a well rounded person on many topics. Though those topics have to be
    interesting to me at least.</p>
    
    <p>Stick around for further developments.</p>', 
     NOW(), false);