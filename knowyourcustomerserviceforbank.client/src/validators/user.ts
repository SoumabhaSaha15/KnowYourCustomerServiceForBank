import z from "zod";

export const userResponse = z.strictObject({
  userId: z.int().positive(),
  fullName: z.string().regex(/^[a-zA-Z\s'.-]+$/).min(2).max(100),
  email: z.email().max(254),
  phoneNumber: z.string().regex(/^\+?[1-9]\d{1,14}$/).max(20).min(10).nullable(),
  address: z.string().regex(/^[a-zA-Z\s'.-]+$/).max(256).nullable(),
  onboardingStatus: z.enum(["NEW", "IN_PROGRESS", "COMPLETED", "NOT_APPLICABLE"]),
  dateOfBirth: z.iso.date().nullable(),
  userRole: z.enum(["CUSTOMER", "ADMIN", "KYC_OFFICER", "COMPLIANCE_OFFICER"]),
  isActive: z.boolean(),
  createdAt: z.iso.datetime({ local: true, precision: 7 }),
  updatedAt: z.iso.datetime({ local: true, precision: 7 })
});
export const refinedUserResponse = userResponse.superRefine((data, ctx) => {
  if (data.userRole === "CUSTOMER") {
    if (!data.phoneNumber) {
      ctx.addIssue({
        code: "custom",
        message: "Phone number is required for customers.",
        path: ["phoneNumber"],
      });
    }

    if (!data.address) {
      ctx.addIssue({
        code: "custom",
        message: "Address is required for customers.",
        path: ["address"],
      });
    }

    if (data.onboardingStatus === "NOT_APPLICABLE") {
      ctx.addIssue({
        code: "custom",
        message: "Onboarding status can't be 'NOT_APPLICABLE'.",
        path: ["onboardingStatus"],
      });
    }

    if (!data.dateOfBirth) {
      ctx.addIssue({
        code: "custom",
        message: "Date of birth is required.",
        path: ["dateOfBirth"],
      });
    } else if (new Date(data.dateOfBirth) >= new Date()) {
      ctx.addIssue({
        code: "custom",
        message: "Date of birth must be in the past.",
        path: ["dateOfBirth"],
      });
    }
  }
});

export const createUser = userResponse.omit({ userId: true });

export const userLogin = userResponse
  .pick({ email: true })
  .extend({ password: z.string().min(8) });

export type UserResponseType = z.infer<typeof userResponse>;
export type UserLoginType = z.infer<typeof userLogin>;
export type CreateUserType = z.infer<typeof createUser>;