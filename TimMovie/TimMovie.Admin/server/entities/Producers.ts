import { Column, Entity, Index, ManyToMany } from "typeorm";
import { Films } from "./Films";

@Index("PK_Producers", ["id"], { unique: true })
@Entity("Producers", { schema: "public" })
export class Producers {
  @Column("uuid", { primary: true, name: "Id" })
  id: string;

  @Column("character varying", { name: "Name", length: 100 })
  name: string;

  @Column("character varying", { name: "Surname", nullable: true, length: 100 })
  surname: string | null;

  @Column("text", { name: "Photo", nullable: true })
  photo: string | null;

  @ManyToMany(() => Films, (films) => films.producers)
  films: Films[];
}
