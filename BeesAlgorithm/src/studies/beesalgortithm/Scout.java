package studies.beesalgortithm;

import java.util.Random;

public class Scout implements Comparable<Scout> {

	private Flower flower = new Flower();
	
	public Scout() {}
	
	public Scout(Flower flower) {
		super();
		this.flower = flower;
	}

	public void choseFlower() {
		flower.choseFlower();
	}

	@Override
	public int compareTo(Scout scout) {
		if(this.flower.getSolutionValue() > scout.flower.getSolutionValue()){
			return 1;	
		} 
		else if(this.flower.getSolutionValue() == scout.flower.getSolutionValue()){
			return 0;	
		} else{
			return -1;	
		}
	}

	public Flower getFlower() {
		return flower;
	}

	public void setFlower(Flower flower) {
		this.flower = flower;
	}
}
