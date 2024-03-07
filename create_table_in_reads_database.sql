-- before executing this script create a database by running
-- CREATE DATABASE khorikov_cqrs_reads;
-- and then connecting to that database.

create table students
(
    student_id            bigint not null
        primary key,
    name                  varchar(50),
    email                 varchar(50),
    number_of_enrollments integer,
    first_course_name     varchar(50),
    first_course_credits  integer,
    first_course_grade    integer,
    second_course_name    varchar(50),
    second_course_credits integer,
    second_course_grade   integer
);

alter table students
    owner to postgres;