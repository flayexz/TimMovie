import { Column, Entity, Index, OneToMany } from "typeorm";
import { AspNetUsers } from "./AspNetUsers";
import { Films } from "./Films";

@Index("PK_Countries", ["id"], { unique: true })
@Entity("Countries", { schema: "public" })
export class Countries {
  @Column("uuid", { primary: true, name: "Id" })
  id: string;

  @Column("character varying", { name: "Name", length: 100 })
  name: string;

  @OneToMany(() => AspNetUsers, (aspNetUsers) => aspNetUsers.country)
  aspNetUsers: AspNetUsers[];

  @OneToMany(() => Films, (films) => films.country)
  films: Films[];
}
