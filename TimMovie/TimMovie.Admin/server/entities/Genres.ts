import { Column, Entity, Index, ManyToMany } from "typeorm";
import { Films } from "./Films";

@Index("PK_Genres", ["id"], { unique: true })
@Entity("Genres", { schema: "public" })
export class Genres {
  @Column("uuid", { primary: true, name: "Id" })
  id: string;

  @Column("character varying", { name: "Name", length: 50 })
  name: string;

  @ManyToMany(() => Films, (films) => films.genres)
  films: Films[];
}
