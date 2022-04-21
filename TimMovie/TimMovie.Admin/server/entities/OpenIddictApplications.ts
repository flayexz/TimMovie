import { Column, Entity, Index, OneToMany } from "typeorm";
import { OpenIddictAuthorizations } from "./OpenIddictAuthorizations";
import { OpenIddictTokens } from "./OpenIddictTokens";

@Index("IX_OpenIddictApplications_ClientId", ["clientId"], { unique: true })
@Index("PK_OpenIddictApplications", ["id"], { unique: true })
@Entity("OpenIddictApplications", { schema: "public" })
export class OpenIddictApplications {
  @Column("text", { primary: true, name: "Id" })
  id: string;

  @Column("character varying", {
    name: "ClientId",
    nullable: true,
    length: 100,
  })
  clientId: string | null;

  @Column("text", { name: "ClientSecret", nullable: true })
  clientSecret: string | null;

  @Column("character varying", {
    name: "ConcurrencyToken",
    nullable: true,
    length: 50,
  })
  concurrencyToken: string | null;

  @Column("character varying", {
    name: "ConsentType",
    nullable: true,
    length: 50,
  })
  consentType: string | null;

  @Column("text", { name: "DisplayName", nullable: true })
  displayName: string | null;

  @Column("text", { name: "DisplayNames", nullable: true })
  displayNames: string | null;

  @Column("text", { name: "Permissions", nullable: true })
  permissions: string | null;

  @Column("text", { name: "PostLogoutRedirectUris", nullable: true })
  postLogoutRedirectUris: string | null;

  @Column("text", { name: "Properties", nullable: true })
  properties: string | null;

  @Column("text", { name: "RedirectUris", nullable: true })
  redirectUris: string | null;

  @Column("text", { name: "Requirements", nullable: true })
  requirements: string | null;

  @Column("character varying", { name: "Type", nullable: true, length: 50 })
  type: string | null;

  @OneToMany(
    () => OpenIddictAuthorizations,
    (openIddictAuthorizations) => openIddictAuthorizations.application
  )
  openIddictAuthorizations: OpenIddictAuthorizations[];

  @OneToMany(
    () => OpenIddictTokens,
    (openIddictTokens) => openIddictTokens.application
  )
  openIddictTokens: OpenIddictTokens[];
}
