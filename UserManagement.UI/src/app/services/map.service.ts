import { Injectable } from '@angular/core';
import L from 'leaflet';

@Injectable({
  providedIn: 'root',
})
export class MapService {
  private map: any;
  private tiles: any;
  private marker: any;
  private icon: any;

  private zoom: number = 12;
  private maxZoom: number = 18;
  private minZoom: number = 3;
  private mapId: string = 'map';
  private zoomControl: boolean = true;
  private center: [number, number] = [14.6091, 121.0223];

  private tileUrl = 'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png';
  private attribution: string =
    '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>';

  constructor() {
    this.icon = new L.Icon({
      iconUrl: `http://leafletjs.com/examples/custom-icons/leaf-green.png`,
      shadowSize: [0, 0],
    });
  }

  public get Map() {
    return this.map;
  }

  public get Tiles() {
    return this.tiles;
  }

  public setCenter(latitude: number, longitude: number) {
    this.center = [latitude, longitude];
  }
  public get Marker() {
    return this.marker;
  }

  public createMap() {
    this.map = L.map(this.mapId, {
      center: this.center,
      zoom: this.zoom,
      zoomControl: this.zoomControl,
    });
  }

  public createTileLayer() {
    this.tiles = L.tileLayer(this.tileUrl, {
      maxZoom: this.maxZoom,
      minZoom: this.minZoom,
      attribution: this.attribution,
    });
  }

  public createMarker(latitude: number, longitude: number) {
    this.marker = L.marker([latitude, longitude], { icon: this.icon });
  }

  public markPosition(latitude: any, longitude: any) {
    this.map.on('click', (e: any) => {
      if (this.marker) this.map.removeLayer(this.marker);

      this.createMarker(e.latlng.lat, e.latlng.lng);
      latitude.setValue(e.latlng.lat.toString());
      longitude.setValue(e.latlng.lng.toString());

      this.map.addLayer(this.marker);
      this.marker.addTo(this.map);
    });
  }

  public mapAddToTiles() {
    this.tiles.addTo(this.map);
  }

  public markerAddToMap() {
    this.marker.addTo(this.map);
  }
  public disableMap() {
    this.map.dragging.disable();
    this.map.touchZoom.disable();
    this.map.doubleClickZoom.disable();
    this.map.scrollWheelZoom.disable();
    this.map.boxZoom.disable();
    this.map.keyboard.disable();
  }

  public enableMap() {
    this.map.dragging.enable();
    this.map.touchZoom.enable();
    this.map.doubleClickZoom.enable();
    this.map.scrollWheelZoom.enable();
    this.map.boxZoom.enable();
    this.map.keyboard.enable();
  }
}
