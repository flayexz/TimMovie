import {
  Column,
  Entity,
  Index,
  JoinColumn,
  JoinTable,
  ManyToMany,
  ManyToOne,
  OneToMany,
} from "typeorm";
import { Actors } from "./Actors";
import { AspNetUsers } from "./AspNetUsers";
import { Banners } from "./Banners";
import { Comments } from "./Comments";
import { Genres } from "./Genres";
import { Producers } from "./Producers";
import { Subscribes } from "./Subscribes";
import { Countries } from "./Countries";
import { WatchedFilms } from "./WatchedFilms";

@Index("IX_Films_CountryId", ["countryId"], {})
@Index("PK_Films", ["id"], { unique: true })
@Entity("Films", { schema: "public" })
export class Films {
  @Column("uuid", { primary: true, name: "Id" })
  id: string;

  @Column("character varying", { name: "Title", length: 200 })
  title: string;

  @Column("text", { name: "Description", nullable: true })
  description: string | null;

  @Column("uuid", { name: "CountryId", nullable: true })
  countryId: string | null;

  @Column("text", { name: "Image", nullable: true })
  image: string | null;

  @Column("text", { name: "FilmLink", nullable: true })
  filmLink: string | null;

  @Column("integer", { name: "Year", default: () => "0" })
  year: number;

  @ManyToMany(() => Actors, (actors) => actors.films)
  actors: Actors[];

  @OneToMany(() => AspNetUsers, (aspNetUsers) => aspNetUsers.watchingFilm)
  aspNetUsers: AspNetUsers[];

  @OneToMany(() => Banners, (banners) => banners.film)
  banners: Banners[];

  @OneToMany(() => Comments, (comments) => comments.film)
  comments: Comments[];

  @ManyToMany(() => Genres, (genres) => genres.films)
  @JoinTable({
    name: "FilmGenre",
    joinColumns: [{ name: "FilmsId", referencedColumnName: "id" }],
    inverseJoinColumns: [{ name: "GenresId", referencedColumnName: "id" }],
    schema: "public",
  })
  genres: Genres[];

  @ManyToMany(() => Producers, (producers) => producers.films)
  @JoinTable({
    name: "FilmProducer",
    joinColumns: [{ name: "FilmsId", referencedColumnName: "id" }],
    inverseJoinColumns: [{ name: "ProducersId", referencedColumnName: "id" }],
    schema: "public",
  })
  producers: Producers[];

  @ManyToMany(() => Subscribes, (subscribes) => subscribes.films)
  @JoinTable({
    name: "FilmSubscribe",
    joinColumns: [{ name: "FilmsId", referencedColumnName: "id" }],
    inverseJoinColumns: [{ name: "SubscribesId", referencedColumnName: "id" }],
    schema: "public",
  })
  subscribes: Subscribes[];

  @ManyToMany(() => AspNetUsers, (aspNetUsers) => aspNetUsers.films)
  @JoinTable({
    name: "FilmUser",
    joinColumns: [{ name: "FilmsWatchLaterId", referencedColumnName: "id" }],
    inverseJoinColumns: [
      { name: "UsersWatchLaterId", referencedColumnName: "id" },
    ],
    schema: "public",
  })
  aspNetUsers2: AspNetUsers[];

  @ManyToOne(() => Countries, (countries) => countries.films, {
    onDelete: "SET NULL",
  })
  @JoinColumn([{ name: "CountryId", referencedColumnName: "id" }])
  country: Countries;

  @OneToMany(() => WatchedFilms, (watchedFilms) => watchedFilms.film)
  watchedFilms: WatchedFilms[];
}
