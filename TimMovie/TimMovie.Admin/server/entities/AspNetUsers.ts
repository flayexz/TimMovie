import {
  Column,
  Entity,
  Index,
  JoinColumn,
  ManyToMany,
  ManyToOne,
  OneToMany,
} from "typeorm";
import { AspNetUserClaims } from "./AspNetUserClaims";
import { AspNetUserLogins } from "./AspNetUserLogins";
import { AspNetRoles } from "./AspNetRoles";
import { AspNetUserTokens } from "./AspNetUserTokens";
import { Countries } from "./Countries";
import { Films } from "./Films";
import { CommentReports } from "./CommentReports";
import { Comments } from "./Comments";
import { Notifications } from "./Notifications";
import { UserSubscribes } from "./UserSubscribes";
import { WatchedFilms } from "./WatchedFilms";

@Index("IX_AspNetUsers_CountryId", ["countryId"], {})
@Index("PK_AspNetUsers", ["id"], { unique: true })
@Index("EmailIndex", ["normalizedEmail"], {})
@Index("UserNameIndex", ["normalizedUserName"], { unique: true })
@Index("IX_AspNetUsers_WatchingFilmId", ["watchingFilmId"], {})
@Entity("AspNetUsers", { schema: "public" })
export class AspNetUsers {
  @Column("uuid", { primary: true, name: "Id" })
  id: string;

  @Column("integer", { name: "Status" })
  status: number;

  @Column("uuid", { name: "WatchingFilmId", nullable: true })
  watchingFilmId: string | null;

  @Column("uuid", { name: "CountryId", nullable: true })
  countryId: string | null;

  @Column("timestamp with time zone", { name: "RegistrationDate" })
  registrationDate: Date;

  @Column("character varying", {
    name: "UserName",
    nullable: true,
    length: 256,
  })
  userName: string | null;

  @Column("character varying", {
    name: "NormalizedUserName",
    nullable: true,
    length: 256,
  })
  normalizedUserName: string | null;

  @Column("character varying", { name: "Email", nullable: true, length: 256 })
  email: string | null;

  @Column("character varying", {
    name: "NormalizedEmail",
    nullable: true,
    length: 256,
  })
  normalizedEmail: string | null;

  @Column("boolean", { name: "EmailConfirmed" })
  emailConfirmed: boolean;

  @Column("text", { name: "PasswordHash", nullable: true })
  passwordHash: string | null;

  @Column("text", { name: "SecurityStamp", nullable: true })
  securityStamp: string | null;

  @Column("text", { name: "ConcurrencyStamp", nullable: true })
  concurrencyStamp: string | null;

  @Column("text", { name: "PhoneNumber", nullable: true })
  phoneNumber: string | null;

  @Column("boolean", { name: "PhoneNumberConfirmed" })
  phoneNumberConfirmed: boolean;

  @Column("boolean", { name: "TwoFactorEnabled" })
  twoFactorEnabled: boolean;

  @Column("timestamp with time zone", { name: "LockoutEnd", nullable: true })
  lockoutEnd: Date | null;

  @Column("boolean", { name: "LockoutEnabled" })
  lockoutEnabled: boolean;

  @Column("integer", { name: "AccessFailedCount" })
  accessFailedCount: number;

  @Column("date", { name: "BirthDate", default: () => "'-infinity'" })
  birthDate: string;

  @Column("character varying", {
    name: "DisplayName",
    length: 100,
    default: () => "''",
  })
  displayName: string;

  @OneToMany(
    () => AspNetUserClaims,
    (aspNetUserClaims) => aspNetUserClaims.user
  )
  aspNetUserClaims: AspNetUserClaims[];

  @OneToMany(
    () => AspNetUserLogins,
    (aspNetUserLogins) => aspNetUserLogins.user
  )
  aspNetUserLogins: AspNetUserLogins[];

  @ManyToMany(() => AspNetRoles, (aspNetRoles) => aspNetRoles.aspNetUsers)
  aspNetRoles: AspNetRoles[];

  @OneToMany(
    () => AspNetUserTokens,
    (aspNetUserTokens) => aspNetUserTokens.user
  )
  aspNetUserTokens: AspNetUserTokens[];

  @ManyToOne(() => Countries, (countries) => countries.aspNetUsers, {
    onDelete: "SET NULL",
  })
  @JoinColumn([{ name: "CountryId", referencedColumnName: "id" }])
  country: Countries;

  @ManyToOne(() => Films, (films) => films.aspNetUsers, {
    onDelete: "SET NULL",
  })
  @JoinColumn([{ name: "WatchingFilmId", referencedColumnName: "id" }])
  watchingFilm: Films;

  @OneToMany(() => CommentReports, (commentReports) => commentReports.user)
  commentReports: CommentReports[];

  @OneToMany(() => Comments, (comments) => comments.author)
  comments: Comments[];

  @ManyToMany(() => Films, (films) => films.aspNetUsers2)
  films: Films[];

  @ManyToMany(() => Notifications, (notifications) => notifications.aspNetUsers)
  notifications: Notifications[];

  @OneToMany(
    () => UserSubscribes,
    (userSubscribes) => userSubscribes.subscribedUser
  )
  userSubscribes: UserSubscribes[];

  @OneToMany(() => WatchedFilms, (watchedFilms) => watchedFilms.watchedUser)
  watchedFilms: WatchedFilms[];
}
