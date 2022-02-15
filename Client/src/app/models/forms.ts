export type LoginForm = {
  username: string;
  password: string;
};

export enum RegisterFields {
  Username = 'username',
  Password = 'password',
  ConfirmPassword = 'confirmPassword',
  Gender = 'gender',
  DateOfBirth = 'dateOfBirth',
  Country = 'country',
  City = 'city',
  KnownAs = 'knownAs',
}
