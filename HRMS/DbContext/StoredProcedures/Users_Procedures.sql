DELIMITER //

CREATE PROCEDURE prcAddUser(
    IN p_Username VARCHAR(100),
    IN p_PasswordHash VARCHAR(255),
    IN p_FirstName VARCHAR(100),
    IN p_LastName VARCHAR(100),
    IN p_Email VARCHAR(150),
    IN p_Phone VARCHAR(50),
    IN p_RoleID INT,
    IN p_User_Status VARCHAR(50),
    IN p_CreatedAt DATETIME,
    IN p_UpdatedAt DATETIME
)
BEGIN
    INSERT INTO Users (
        Username,
        PasswordHash,
        FirstName,
        LastName,
        Email,
        Phone,
        RoleID,
        User_Status,
        CreatedAt,
        UpdatedAt
    )
    VALUES (
        p_Username,
        p_PasswordHash,
        p_FirstName,
        p_LastName,
        p_Email,
        p_Phone,
        p_RoleID,
        p_User_Status,
        p_CreatedAt,
        p_UpdatedAt
    );

    SELECT LAST_INSERT_ID() AS UserID;
END //

CREATE PROCEDURE prcUpdateUser(
    IN p_UserID INT,
    IN p_FirstName VARCHAR(100),
    IN p_LastName VARCHAR(100),
    IN p_Email VARCHAR(150),
    IN p_Phone VARCHAR(50),
    IN p_RoleID INT,
    IN p_User_Status VARCHAR(50),
    IN p_UpdatedAt DATETIME
)
BEGIN
    UPDATE Users
    SET
        FirstName = p_FirstName,
        LastName = p_LastName,
        Email = p_Email,
        Phone = p_Phone,
        RoleID = p_RoleID,
        User_Status = p_User_Status,
        UpdatedAt = p_UpdatedAt
    WHERE UserID = p_UserID;
END //

CREATE PROCEDURE prcUpdateUserPassword(
    IN p_UserID INT,
    IN p_PasswordHash VARCHAR(255),
    IN p_UpdatedAt DATETIME
)
BEGIN
    UPDATE Users
    SET
        PasswordHash = p_PasswordHash,
        UpdatedAt = p_UpdatedAt
    WHERE UserID = p_UserID;
END //

CREATE PROCEDURE prcDeleteUser(
    IN p_UserID INT
)
BEGIN
    DELETE FROM Users WHERE UserID = p_UserID;
END //

CREATE PROCEDURE prcGetUserById(
    IN p_UserID INT
)
BEGIN
    SELECT u.*, r.RoleName
    FROM Users u
    INNER JOIN Roles r ON u.RoleID = r.RoleID
    WHERE u.UserID = p_UserID;
END //

CREATE PROCEDURE prcGetAllUsers()
BEGIN
    SELECT u.*, r.RoleName
    FROM Users u
    INNER JOIN Roles r ON u.RoleID = r.RoleID;
END //

CREATE PROCEDURE prcGetUserGridData()
BEGIN
    SELECT u.*, r.RoleName
    FROM Users u
    INNER JOIN Roles r ON u.RoleID = r.RoleID
    ORDER BY u.CreatedAt DESC;
END //

CREATE PROCEDURE prcSearchUsers(
    IN p_Keyword VARCHAR(255)
)
BEGIN
    SELECT u.UserID,
           u.Username,
           u.FirstName,
           u.LastName,
           u.Email,
           u.Phone,
           u.User_Status,
           u.CreatedAt,
           u.UpdatedAt,
           r.RoleName,
           u.RoleID,
           u.PasswordHash
    FROM Users u
    JOIN Roles r ON u.RoleID = r.RoleID
    WHERE u.Username LIKE CONCAT('%', p_Keyword, '%')
       OR u.FirstName LIKE CONCAT('%', p_Keyword, '%')
       OR u.LastName LIKE CONCAT('%', p_Keyword, '%')
       OR u.Email LIKE CONCAT('%', p_Keyword, '%')
       OR u.Phone LIKE CONCAT('%', p_Keyword, '%')
       OR u.User_Status LIKE CONCAT('%', p_Keyword, '%')
       OR r.RoleName LIKE CONCAT('%', p_Keyword, '%')
    ORDER BY u.UserID;
END //

CREATE PROCEDURE prcGetUserStatusCounts()
BEGIN
    SELECT
        COUNT(*) as Total,
        SUM(CASE WHEN User_Status = 'Active' THEN 1 ELSE 0 END) as Active,
        SUM(CASE WHEN User_Status = 'Inactive' THEN 1 ELSE 0 END) as Inactive,
        SUM(CASE WHEN User_Status = 'Suspended' THEN 1 ELSE 0 END) as Suspended
    FROM Users;
END //

CREATE PROCEDURE prcAuthenticateUser(
    IN p_Username VARCHAR(100),
    IN p_PasswordHash VARCHAR(255)
)
BEGIN
    SELECT u.*, r.RoleName
    FROM Users u
    INNER JOIN Roles r ON u.RoleID = r.RoleID
    WHERE u.Username = p_Username
      AND u.PasswordHash = p_PasswordHash
      AND (u.User_Status IS NULL OR u.User_Status <> 'Inactive')
    LIMIT 1;
END //

DELIMITER ;
