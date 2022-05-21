import {Raw} from "typeorm";

export function includeNamePart(namePart: string){
    let lowerNamePart = namePart.toLowerCase();
    
    return Raw(alias => `lower(${alias}) LIKE '%${lowerNamePart}%'`);
}