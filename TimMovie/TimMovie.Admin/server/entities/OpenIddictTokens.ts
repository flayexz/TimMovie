import { Column, Entity, Index, JoinColumn, ManyToOne } from "typeorm";
import { OpenIddictApplications } from "./OpenIddictApplications";
import { OpenIddictAuthorizations } from "./OpenIddictAuthorizations";

@Index(
  "IX_OpenIddictTokens_ApplicationId_Status_Subject_Type",
  ["applicationId", "status", "subject", "type"],
  {}
)
@Index("IX_OpenIddictTokens_AuthorizationId", ["authorizationId"], {})
@Index("PK_OpenIddictTokens", ["id"], { unique: true })
@Index("IX_OpenIddictTokens_ReferenceId", ["referenceId"], { unique: true })
@Entity("OpenIddictTokens", { schema: "public" })
export class OpenIddictTokens {
  @Column("text", { primary: true, name: "Id" })
  id: string;

  @Column("text", { name: "ApplicationId", nullable: true })
  applicationId: string | null;

  @Column("text", { name: "AuthorizationId", nullable: true })
  authorizationId: string | null;

  @Column("character varying", {
    name: "ConcurrencyToken",
    nullable: true,
    length: 50,
  })
  concurrencyToken: string | null;

  @Column("timestamp without time zone", {
    name: "CreationDate",
    nullable: true,
  })
  creationDate: Date | null;

  @Column("timestamp without time zone", {
    name: "ExpirationDate",
    nullable: true,
  })
  expirationDate: Date | null;

  @Column("text", { name: "Payload", nullable: true })
  payload: string | null;

  @Column("text", { name: "Properties", nullable: true })
  properties: string | null;

  @Column("timestamp without time zone", {
    name: "RedemptionDate",
    nullable: true,
  })
  redemptionDate: Date | null;

  @Column("character varying", {
    name: "ReferenceId",
    nullable: true,
    length: 100,
  })
  referenceId: string | null;

  @Column("character varying", { name: "Status", nullable: true, length: 50 })
  status: string | null;

  @Column("character varying", { name: "Subject", nullable: true, length: 400 })
  subject: string | null;

  @Column("character varying", { name: "Type", nullable: true, length: 50 })
  type: string | null;

  @ManyToOne(
    () => OpenIddictApplications,
    (openIddictApplications) => openIddictApplications.openIddictTokens
  )
  @JoinColumn([{ name: "ApplicationId", referencedColumnName: "id" }])
  application: OpenIddictApplications;

  @ManyToOne(
    () => OpenIddictAuthorizations,
    (openIddictAuthorizations) => openIddictAuthorizations.openIddictTokens
  )
  @JoinColumn([{ name: "AuthorizationId", referencedColumnName: "id" }])
  authorization: OpenIddictAuthorizations;
}
