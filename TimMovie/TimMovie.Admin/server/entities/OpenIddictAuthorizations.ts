import {
  Column,
  Entity,
  Index,
  JoinColumn,
  ManyToOne,
  OneToMany,
} from "typeorm";
import { OpenIddictApplications } from "./OpenIddictApplications";
import { OpenIddictTokens } from "./OpenIddictTokens";

@Index(
  "IX_OpenIddictAuthorizations_ApplicationId_Status_Subject_Type",
  ["applicationId", "status", "subject", "type"],
  {}
)
@Index("PK_OpenIddictAuthorizations", ["id"], { unique: true })
@Entity("OpenIddictAuthorizations", { schema: "public" })
export class OpenIddictAuthorizations {
  @Column("text", { primary: true, name: "Id" })
  id: string;

  @Column("text", { name: "ApplicationId", nullable: true })
  applicationId: string | null;

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

  @Column("text", { name: "Properties", nullable: true })
  properties: string | null;

  @Column("text", { name: "Scopes", nullable: true })
  scopes: string | null;

  @Column("character varying", { name: "Status", nullable: true, length: 50 })
  status: string | null;

  @Column("character varying", { name: "Subject", nullable: true, length: 400 })
  subject: string | null;

  @Column("character varying", { name: "Type", nullable: true, length: 50 })
  type: string | null;

  @ManyToOne(
    () => OpenIddictApplications,
    (openIddictApplications) => openIddictApplications.openIddictAuthorizations
  )
  @JoinColumn([{ name: "ApplicationId", referencedColumnName: "id" }])
  application: OpenIddictApplications;

  @OneToMany(
    () => OpenIddictTokens,
    (openIddictTokens) => openIddictTokens.authorization
  )
  openIddictTokens: OpenIddictTokens[];
}
