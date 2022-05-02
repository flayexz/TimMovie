import { Column, Entity, Index, ManyToMany, OneToMany } from "typeorm";
import { Film } from "./Film";
import { UserSubscribe } from "./UserSubscribe";

@Index("PK_Subscribes", ["id"], { unique: true })
@Entity("Subscribes", { schema: "public" })
export class Subscribe {
  @Column("uuid", { primary: true, name: "Id" })
  id: string;

  @Column("character varying", { name: "Name", length: 70 })
  name: string;

  @Column("real", { name: "Price", precision: 24 })
  price: number;

  @Column("text", { name: "Description", nullable: true })
  description: string | null;

  @ManyToMany(() => Film, (films) => films.subscribes)
  films: Film[];

  @OneToMany(() => UserSubscribe, (userSubscribes) => userSubscribes.subscribe)
  userSubscribes: UserSubscribe[];
}
