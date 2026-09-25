import z from "zod";

export const userResponse = z.strictObject({
  userId: z.int().positive(),
  fullName: z.string().regex(/^[a-zA-Z\s'.\-]+$/).min(2).max(100),
  email: z.email().max(254),
  phoneNumber: z.string().regex(/^\+?[1-9]\d{1,14}$/).max(20).min(10).nullable(),
  address: z.string().regex(/^[a-zA-Z\s'.\-]+$/).max(256).nullable(),
  onboardingStatus: z.enum(["NEW", "IN_PROGRESS", "COMPLETED", "NOT_APPLICABLE"]),
  dateOfBirth: z.iso.date().nullable(),
  userRole: z.enum(["CUSTOMER", "ADMIN", "KYC_OFFICER", "COMPLIANCE_OFFICER"]),
  isActive: z.boolean(),
  createdAt: z.iso.datetime(),
  updatedAt: z.iso.datetime()
})
  .refine(
    (value) => (value.userRole === "CUSTOMER") ? value.phoneNumber !== null : true,
    { error: "Phone number can't be null for customer.", path: ["phoneNumber"] }
  )
  .refine(
    (value) => (value.userRole === "CUSTOMER") ? value.dateOfBirth !== null : true,
    { error: "DOB can't be null for customer.", path: ["dateOfBirth"] }
  )
  .refine(
    (value) => (value.userRole === "CUSTOMER") ? value.address !== null : true,
    { error: "Address can't be null for customer.", path: ["address"] }
  );

export const createUser = userResponse.omit({ userId: true });

export const userLogin = userResponse
  .pick({ email: true })
  .extend({ password: z.string().min(8) });

export type UserResponseType = z.infer<typeof userResponse>;
export type UserLoginType = z.infer<typeof userLogin>;
export type CreateUserType = z.infer<typeof createUser>;