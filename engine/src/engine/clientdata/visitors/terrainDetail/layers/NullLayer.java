package engine.clientdata.visitors.terrainDetail.layers;

import org.apache.mina.core.buffer.IoBuffer;

import engine.clientdata.visitors.TerrainVisitor;

public class NullLayer extends Layer {

	@Override
	public void loadData(IoBuffer buffer) throws Exception {}

	@Override
	public float process(float x, float y, float transform_value, float base_value, TerrainVisitor ti) {
		return base_value;
	}

}
