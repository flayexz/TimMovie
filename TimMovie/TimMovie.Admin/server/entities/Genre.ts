import { Column, Entity, Index, ManyToMany } from "typeorm";
import { Film } from "./Film";

@Index("PK_Genres", ["id"], { unique: true })
@Entity("Genres", { schema: "public" })
export class Genre {
  @Column("uuid", { primary: true, name: "Id" })
  id: string;

  @Column("character varying", { name: "Name", length: 50 })
  name: string;

  @ManyToMany(() => Film, (films) => films.genres)
  films: Film[];
}
