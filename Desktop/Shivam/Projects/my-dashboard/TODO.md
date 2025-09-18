# TODO: Implement Signup and Login with MongoDB Hashed Credentials

## Backend Tasks
- [x] Add Password field to User.cs model
- [x] Install BCrypt.Net-Next NuGet package for password hashing
- [x] Update UserController.cs: Add Signup endpoint (hash password, store user)
- [x] Update UserController.cs: Add Login endpoint (verify credentials, return JWT)
- [x] Update Program.cs: Configure JWT services and authentication

## Frontend Tasks
- [ ] Create SignupComponent in Angular
- [ ] Create LoginComponent in Angular
- [ ] Update app.routes.ts to include /signup and /login routes
- [ ] Update app.html or main component for navigation links

## Followup
- [ ] Test backend endpoints
- [ ] Test frontend integration
- [ ] Run the application and verify functionality
